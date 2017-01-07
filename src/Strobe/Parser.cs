using System.Collections.Generic;
namespace Strobe
{
	public class Parser
	{
		List<SToken> Input;
		ParseTree Tree;
		Result Res;
		Instruction Instruction_current;
		Namespace Namespace_current;
		Function Function_current;
		int Current;
		SToken Now;

		// Data
		List<string> PreProcessor = new List<string>();
		List<Namespace> Namespaces = new List<Namespace>();

		public Parser(List<SToken> input)
		{
			Current = 0;
			Res = new Result();
			Tree = new ParseTree();
			Input = input;
			Clean();
			Parse();
		}

		bool inNamespace, inFunction, inNamespaceBlock, inFunctionBlock, needArgs;

		void Clean()
		{
			inFunctionBlock = false;
			inNamespaceBlock = false;
			inNamespace = false;
			inFunction = false;
			needArgs = false;
		}
		void Parse()
		{
			while (Current < Input.Count) {
				Now = Input [Current];
				if (Now.Type == STokenType.PreProcessor) {
					PreProcessor.Add (Now.Value);
					Current++;
					continue;
				}
				if (!inNamespace) {
					if (CheckNamespace ()) {
						continue;
					}
				} else {
					if (!inNamespaceBlock) {
						if (Now.Type == STokenType.Block && Now.Value == "open") {
							Now = Input [++Current];
							inNamespaceBlock = true;
							continue;
						}
					} else {
						if (!inFunction) {
							if (CheckFunction ()) {
								continue;
							}
						} else {
							if (!inFunctionBlock) {
								if (Now.Type == STokenType.Block && Now.Value == "open") {
									Now = Input [++Current];
									inFunctionBlock = true;
									continue;
								}
							} else {
								if (Now.Type == STokenType.Arguments || Now.Type == STokenType.Instruction || Now.Type == STokenType.Return) {
									if (Instruction ()) {
										continue;
									}
								}
							}
						}
					}
				}
				if (Now.Type == STokenType.Block && Now.Value == "close") {
					if (inFunctionBlock) {
						inFunctionBlock = false;
						inFunction = false;
						Current++;
						Namespace_current.Functions.Add (Function_current);
						continue;
					}
						else
					{
						Namespaces.Add (Namespace_current);
						inNamespace = false;
						inNamespaceBlock = false;
						Current++;
						continue;
					}
				}
				Res.Errors.Add(new Error { Code = 4, Value = "Unexpected Token (" + Now.Type + ":" + Now.Value + ")", Location = Now.Location });
				break;
			}
			Tree.Namespaces = Namespaces;
			Tree.Preprocessor = PreProcessor;
		}
		bool Instruction()
		{
			if (Now.Type == STokenType.Arguments) {
				if (needArgs) {
					Instruction_current.Func.Arguments = parseArguments (Now);
					Function_current.Instructions.Add (Instruction_current);
					needArgs = false;
					Current++;
					return true;
				} else {
					return false;
				}
			}
			if(Now.Type == STokenType.Instruction)
			{
				needArgs = true;
				InstructionType type = InstructionType.Void;
				Token Tfunc = new Token();
				Token Top = new Token();
				Variable Tvar = new Variable();

				bool Lvar = false;
				bool Lop = false;
				bool Lfunc = false;

				foreach (Token x in Now.Instruction) {
					if (x.Type == TokenType.Operator) {
						if (!Lop) {
							Top = x;
							Lop = true;
							continue;
						}
					}
					if (x.Type == TokenType.Identifier) {
						if (!Lfunc) {
							Tfunc = x;
							Lfunc = true;
							continue;
						}
					}
					if (x.Type == TokenType.Variable) {
						if (!Lvar) {
							Tvar = new Variable { Name = x.Value };
							Lvar = true;
							continue;
						}
					}
					if (x.Type == TokenType.Register) {
						if (!Lvar) {
							Tvar = new Variable { Name = x.Value, isReg = true };
							Lvar = true;
							continue;
						}
					}
					if (x.Type == TokenType.Number) {
						if (!Lvar) {
							Tvar = new Variable { Name = x.Value, isNum = true, isConst = true };
							Lvar = true;
							continue;
						}
					}
					if (x.Type == TokenType.String) {
						if (!Lvar) {
							Tvar = new Variable { Name = x.Value, isConst = true };
							Lvar = true;
							continue;
						}
					}
					Res.Errors.Add (new Error {
						Code = 5,
						Value = "Unexpected Token (" + Now.Type + ":" + Now.Value + ")",
						Location = Now.Location
					});
					break;
				}
				string fNamespace = "";
				string fFunction = "";

				string[] tc = Tfunc.Value.Split ('.');
				fFunction = tc [tc.Length - 1];
				for (int f = 0; f < tc.Length - 1; f++) {
					fNamespace = tc[f] + ".";
				}
				if (fNamespace.Length > 1) {
					fNamespace = fNamespace.Substring (0, fNamespace.Length - 1);
				} else {
					fNamespace = Namespace_current.Name;
				}

				Execute func = new Execute () {
					Function = fFunction,
					Namespace = fNamespace,
					Arguments = new Args()
					{
						Arguments = new List<Variable>()
					}
				};

				Operator op = new Operator() {
					Type = Top.Value,
				};

				Instruction_current = new Instruction
				{
					Func = func,
					Op = op,
					Var = Tvar,
					Type = type,
				};
				Current++;
				return true;
			}
			if (Now.Type == STokenType.Return) {
				Function_current.Ret = new Return {
					Type = Now.Instruction [0].Type,
					Value = Now.Instruction [0].Value
				};
				while(!(Now.Type == STokenType.Block && Now.Value == "close"))
				{
					Now = Input [++Current];
				}
				return true;
			}
			return false;
		}

		bool CheckNamespace()
		{
			if (Now.Type == STokenType.Namespace) {
				Namespace_current = new Namespace {
					Name = Now.Value,
					Functions = new List<Function>()
				};
				inNamespace = true;
				Current++;
				return true;
			}
			Current++;
			return false;
		}

		Args parseArguments(SToken args)
		{
			Args n = new Args {
				Arguments = new List<Variable>()
			};
			bool toAdd = true;
			foreach (Token x in args.Instruction) {
				if (toAdd) {
					toAdd = false;
					if (x.Type == TokenType.Variable) {
						n.Arguments.Add (new Variable { Name = x.Value });
					}
					if (x.Type == TokenType.Register) {
						n.Arguments.Add (new Variable { Name = x.Value, isReg = true });
					}
					if (x.Type == TokenType.Number) {
						n.Arguments.Add (new Variable { Name = x.Value, isNum = true, isConst = true });
					}
					if (x.Type == TokenType.String) {
						n.Arguments.Add (new Variable { Name = x.Value, isConst = true });
					}
				} else {
					if (x.Type == TokenType.Operator && x.Value == ",") {
						toAdd = true;
					}
				}
			}
			return n;
		}

		bool CheckFunction()
		{
			if (Now.Type == STokenType.Function) {
				Function_current = new Function {
					Name = Now.Value,
					Instructions = new List<Instruction> (),
					Ret = new Return (),
				};
				Now = Input [++Current];
				if (Now.Type == STokenType.Arguments) {
					Function_current.Arguments = parseArguments (Now);
					inFunction = true;
					Current++;
					return true;
				}
			}
			return false;
		}

		public ParserResult get()
		{
			foreach (Namespace x in Tree.Namespaces) {
				System.Console.WriteLine ("Namespace: {0}",x.Name);
				foreach (Function y in x.Functions)
				{
					System.Console.WriteLine ("+Function: {0}",y.Name);
					foreach (Instruction z in y.Instructions) {
						System.Console.WriteLine ("++Instruction: {0} ", z.Func.Namespace+"."+z.Func.Function);
						System.Console.WriteLine ("+++Variable: ", z.Var.Name);
						System.Console.WriteLine ("+++Operator: ", z.Op.Type);
						foreach (Variable v in z.Func.Arguments.Arguments) {
							System.Console.WriteLine ("++++Argument: " + v.Name);
						}
					}
				}
			}
			return new ParserResult
			{
				Errors = Res.Errors,
				Warnings = Res.Warnings,
				Tree = Tree
			};
		}
	}
}
