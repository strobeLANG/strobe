using System.Text;
using System;
using System.IO;
using System.Collections.Generic;

namespace Strobe
{
	/// <summary>
	/// Code generator.
	/// </summary>
	public class CodeGenerator
	{
		/// <summary>
		/// The Result
		/// </summary>
		Result Res = new Result();

		/// <summary>
		/// The functions.
		/// </summary>
		Dictionary<string,Function> Functions = new Dictionary<string,Function>();

		/// <summary>
		/// The input.
		/// </summary>
		readonly ParseTree Input;

		/// <summary>
		/// The current variable.
		/// </summary>
		int CurrentVar;

		/// <summary>
		/// The variables.
		/// </summary>
		Dictionary<string,int> Vars = new Dictionary<string, int> ();

		/// <summary>
		/// The output.
		/// </summary>
		List<byte> Output = new List<byte>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Strobe.CodeGenerator"/> class.
		/// </summary>
		/// <param name="Input">Input.</param>
		public CodeGenerator (ParseTree Input)
		{
			CurrentVar = 0x2c;
			this.Input = Input;
			Generate();
		}

		/// <summary>
		/// Generate the code.
		/// </summary>
		void Generate()
		{
			foreach (string s in Input.Preprocessor) {
				Preprocess (s);
			}
			foreach (Namespace n in Input.Namespaces) {
				foreach (Function f in n.Functions) {
					Functions.Add (n.Name + f.Name, f);
				}
			}
			Generate (findMain(),null);
		}

		/// <summary>
		/// Preprocess the specified string.
		/// </summary>
		/// <param name="s">String.</param>
		void Preprocess(string s)
		{
			if (s.StartsWith("include")) {
				s = s.TrimStart("include".ToCharArray());
				while(s.StartsWith(" "))
					s = s.TrimStart(" ".ToCharArray());
				if (s.StartsWith ("<")) {
					while (s.EndsWith (" "))
						s = s.TrimEnd (" ".ToCharArray ());
					s = s.TrimEnd (">".ToCharArray ());
					s = s.TrimStart ("<".ToCharArray ());
					s = System.IO.Path.GetDirectoryName (
						System.Reflection.Assembly.GetExecutingAssembly ().Location) + "/lib/" + s;
					var x = new Parser (new Simplifier (new Lexer (File.ReadAllText (s))
							.get ().Tokens).get ().STokens).get ().Tree;
					foreach (Namespace y in x.Namespaces) {
						Input.Namespaces.Add (y);
					}
					foreach (string g in x.Preprocessor) {
						Preprocess (g);
					}
				} else if (s.StartsWith("\"")) {
					while (s.EndsWith (" "))
						s = s.TrimEnd (" ".ToCharArray ());
					s = s.TrimEnd ("\"".ToCharArray ());
					s = s.TrimStart ("\"".ToCharArray ());
					var x = new Parser (new Simplifier (new Lexer (File.ReadAllText (s))
						.get ().Tokens).get ().STokens).get ().Tree;
					foreach (Namespace y in x.Namespaces) {
						Input.Namespaces.Add (y);
					}
					foreach (string g in x.Preprocessor) {
						Preprocess (g);
					}
				}
			}
		}

		/// <summary>
		/// Adds the variable.
		/// </summary>
		/// <param name="Bytes">Bytes.</param>
		int AddVariable(byte[] Bytes)
		{
			int current = ++CurrentVar;
			Output.Add(0x0);
			Output.Add(0x4);
			foreach(byte add in BitConverter.GetBytes(current))
				Output.Add(add);
			Output.Add(0xfe);
			Output.Add((byte)Bytes.Length);
			Output.Add(0xff);
			Output.Add(0x0);
			Output.Add(0x5);
			foreach(byte add in BitConverter.GetBytes(current))
				Output.Add(add);
			Output.Add(0xfe);
			foreach(byte add in Bytes)
				Output.Add(add);
			Output.Add(0xff);
			return current;
		}

		/// <summary>
		/// Interrupt the specified byte.
		/// </summary>
		/// <param name="x">The byte.</param>
		void Interrupt(byte x)
		{
			Output.Add (0x0);
			Output.Add (0x6);
			Output.Add (x);
			Output.Add (0xff);
		}

		/// <summary>
		/// Move the specified addresses.
		/// </summary>
		/// <param name="x">To.</param>
		/// <param name="y">From.</param>
		void Move(int x, int y)
		{
			Output.Add(0x0);
			Output.Add(0x9);
			foreach (byte add in BitConverter.GetBytes(x))
				Output.Add (add);
			Output.Add(0xfe);
			foreach (byte add in BitConverter.GetBytes(y))
				Output.Add (add);
			Output.Add(0xff);
		}

		/// <summary>
		/// Generate from the specified function.
		/// </summary>
		/// <param name="func">Function.</param>
		void Generate(Function func, Args args)
		{
			if (func.Arguments.Arguments.Count != args?.Arguments.Count)
			if (args != null)
				throw new Exception ("Incorrect number of arguments");
			for (int i = 0; i < args?.Arguments.Count; i++) {
				if (args.Arguments [i].isConst) {
					byte[] x;
					if (args.Arguments [i].isNum) {
						x = BitConverter.GetBytes (int.Parse (args.Arguments [i].Name));
					} else {
						x = Encoding.ASCII.GetBytes (args.Arguments [i].Name);
					}
					int var = AddVariable (x);
					if (func.Arguments.Arguments [i].Name.ToLower ().StartsWith ("x")) {
						Move (int.Parse (func.Arguments.Arguments [i].Name.ToLower ().TrimStart ('x')), var);
					} else {
						if (!Vars.ContainsKey (func.Name + func.Arguments.Arguments [i].Name)) {
							Vars.Add (func.Name + func.Arguments.Arguments [i].Name, var);
						} else {
							Vars [func.Name + func.Arguments.Arguments [i].Name] = var;
						}
					}
				} else {
					int var;
					if (func.Arguments.Arguments [i].Name.ToLower ().StartsWith ("x"))
						var = int.Parse (func.Arguments.Arguments [i].Name.ToLower ().TrimStart ('x'));
					else
						var = Vars[func.Name + func.Arguments.Arguments [i].Name];

					if (func.Arguments.Arguments [i].Name.ToLower ().StartsWith ("x")) {
						Move (int.Parse (func.Arguments.Arguments [i].Name.ToLower ().TrimStart ('x')), var);
					} else {
						if (!Vars.ContainsKey (func.Name + func.Arguments.Arguments [i].Name)) {
							Vars.Add (func.Name + func.Arguments.Arguments [i].Name, var);
						} else {
							Vars [func.Name + func.Arguments.Arguments [i].Name] = var;
						}
					}
				}
			}
			foreach (Instruction i in func.Instructions) {
				switch (i.Func.Function) {
				case "new":
					if (i.Func.Arguments.Arguments.Count == 1 && i.Func.Arguments.Arguments [0].isConst) {
						byte[] x;
						if (i.Func.Arguments.Arguments [0].isNum) {
							x = BitConverter.GetBytes(int.Parse (i.Func.Arguments.Arguments [0].Name));
						} else {
							x  = Encoding.ASCII.GetBytes(i.Func.Arguments.Arguments [0].Name);
						}
						int var = AddVariable(x);
						if (i.Op?.Type == "=") {
							if (i.Var?.isConst == false) {
								if (i.Var.Name.ToLower ().StartsWith ("x"))
									Move (int.Parse (i.Var.Name.ToLower ().TrimStart ('x')), var);
								else {
									if (!Vars.ContainsKey (func.Name + i.Var.Name)) {
										Vars.Add (func.Name + i.Var.Name, var);
									} else {
										Vars [func.Name + i.Var.Name] = var;
									}
								}
							}
						}
					} else {
						throw new Exception ("Incorrect amount of arguments in `new`.");
					}
					break;
				case "get":
					if (i.Func.Arguments.Arguments.Count == 1) {
						int var;
						if (i.Func.Arguments.Arguments [0].Name.ToLower ().StartsWith ("x"))
							var = int.Parse (i.Var.Name.ToLower ().TrimStart ('x')); else
						var = Vars[func.Name + i.Func.Arguments.Arguments [0].Name];
						if (i.Op?.Type == "=") {
							if (i.Var?.isConst == false) {
								if (i.Var.Name.ToLower ().StartsWith ("x"))
									Move (int.Parse (i.Var.Name.ToLower ().TrimStart ('x')), var);
								else {
									if (!Vars.ContainsKey (func.Name + i.Var.Name)) {
										Vars.Add (func.Name + i.Var.Name, var);
									} else {
										Vars [func.Name + i.Var.Name] = var;
									}
								}
							}
						}
					} else {
						throw new Exception ("Incorrect amount of arguments in `get`.");
					}
					break;
				case "int":
					if (i.Func.Arguments.Arguments.Count == 1) {
						if (i.Func.Arguments.Arguments [0].isNum) {
							int x = int.Parse (i.Func.Arguments.Arguments [0].Name);
							if (x > 255) {
								throw new Exception ("Interrupt can only accept compile-time constant bytes as arguments.");
							} else {
								Interrupt((byte)x);
							}
						} else {
							throw new Exception ("Interrupt can only accept compile-time constant numbers as arguments.");
						}
					} else {
						throw new Exception ("Incorrect amount of arguments in `interrupt`.");
					}
					break;
				default:
					Function f = find (i.Func.Namespace, i.Func.Function);
					f.Name = i.Func.Namespace + "//" + i.Func.Function;
					Generate (f,i.Func.Arguments);
					if (f.Ret?.Type == TokenType.Variable) {
						int var;
						if (f.Ret.Value.ToLower ().StartsWith ("x"))
							var = int.Parse (f.Ret.Value.ToLower ().TrimStart ('x'));
						else
							var = Vars[f.Name + f.Ret.Value];
						
						if (i.Op?.Type == "=") {
							if (i.Var?.isConst == false) {
								if (i.Var.Name.ToLower ().StartsWith ("x"))
									Move (int.Parse (i.Var.Name.ToLower ().TrimStart ('x')), var);
								else {
									if (!Vars.ContainsKey (func.Name + i.Var.Name)) {
										Vars.Add (func.Name + i.Var.Name, var);
									} else {
										Vars [func.Name + i.Var.Name] = var;
									}
								}
							}
						}
					} else {
						if (f.Ret?.Type == TokenType.Number
							|| f.Ret?.Type == TokenType.Register
							|| f.Ret.Type == TokenType.String)
						throw new Exception ("Functions can only return variables!");
					}
					break;
				}
			}
		}

		/// <summary>
		/// Find the specified function.
		/// </summary>
		/// <param name="namesp">Namespace.</param>
		/// <param name="name">Function.</param>
		Function find(string namesp, string name)
		{
			if (Functions.ContainsKey (namesp + name)) {
				return Functions [namesp + name];
			} else
			throw new Exception ("Unable to find function `"+namesp+"."+name+"`");
		}

		/// <summary>
		/// Finds the main function.
		/// </summary>
		/// <returns>The main.</returns>
		Function findMain()
		{
			foreach (Namespace name in Input.Namespaces)
				foreach (Function func in name.Functions)
					if (func.Name.ToLower () == "main")
						return func;
			throw new Exception("No `Main` function was found!");
		}

		/// <summary>
		/// Get the result.
		/// </summary>
		public CodeGeneratorResult get()
		{
			return new CodeGeneratorResult
			{
				Errors = Res.Errors,
				Warnings = Res.Warnings,
				Code = Output.ToArray(),
			};
		}
	}
}