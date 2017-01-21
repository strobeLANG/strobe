using System.Collections.Generic;
namespace Strobe
{
	/// <summary>
	/// Simplifier.
	/// </summary>
	public class Simplifier
	{
		/// <summary>
		/// The result.
		/// </summary>
		Result Res;

		/// <summary>
		/// The input.
		/// </summary>
		List<Token> Input;

		/// <summary>
		/// The STokens.
		/// </summary>
		List<SToken> STokens;

		/// <summary>
		/// The current location.
		/// </summary>
		int Current;

		/// <summary>
		/// Initializes a new instance of the <see cref="Strobe.Simplifier"/> class.
		/// </summary>
		/// <param name="tokens">Tokens.</param>
		public Simplifier(List<Token> tokens)
		{
			Current = 0;
			Input = tokens;
			STokens = new List<SToken>();
			Res = new Result();
			Simplify();
		}

		/// <summary>
		/// Simplify this instance.
		/// </summary>
		void Simplify()
		{
			while (Current < Input.Count)
			{
				Token Now = Input[Current];
				if (Now.Type == TokenType.Parenthesis)
				{
					if (Now.Value == "{")
					{
						STokens.Add(new SToken { Location = Now.Location, Type = STokenType.Block, Value = "open" });
						Current++;
						continue;
					}
					if (Now.Value == "}")
					{
						STokens.Add(new SToken { Location = Now.Location, Type = STokenType.Block, Value = "close" });
						Current++;
						continue;
					}
					if (Now.Value == "(")
					{
						SToken arg = new SToken
						{
							Location = Now.Location,
							Type = STokenType.Arguments,
							Instruction = new List<Token>()
						};
						while (!(Now.Value == ")"))
						{
							Now = Input[++Current];
							if (Now.Value == ")") {
								break;
							}
							arg.Instruction.Add(Now);
						}
						Current++;
						STokens.Add(arg);
						continue;
					}
				}
				if (Now.Type == TokenType.Identifier || Now.Type == TokenType.Operator || Now.Type == TokenType.Variable)
				{
					if (Now.Value == "namespace")
					{
						if (Input[++Current].Type == TokenType.Identifier)
						{
							STokens.Add(new SToken { Location = Now.Location, Type = STokenType.Namespace, Value = Input[Current].Value });
							Current++;
							continue;
						}
					}

					if (Now.Value == "return")
					{
						STokens.Add(new SToken {
							Location = Now.Location,
							Type = STokenType.Return,
							Instruction = new List<Token> { Input[Current+1] },
							Value = "return" });
						Current += 2;
						if (Input[Current].Type == TokenType.Break)
						{
							Current++;
							continue;
						}
						Res.Errors.Add(new Error { Code = 3, Value = "Unexpected Token in Return (" + Now.Type + ":" + Now.Value + ")", Location = Now.Location });
						break;					}

					if (Now.Value == "function")
					{
						if (Input[++Current].Type == TokenType.Identifier)
						{
							STokens.Add(new SToken { Location = Now.Location, Type = STokenType.Function, Value = Input[Current].Value });
							Current++;
							continue;
						}
					}
					if (Now.Type == TokenType.Variable  || Now.Type == TokenType.Operator || Now.Type == TokenType.Identifier)
					{
						SToken instruc = new SToken
						{
							Location = Now.Location,
							Type = STokenType.Instruction,
							Value = Now.Value,
							Instruction = new List<Token>()
						};
						while (!(Now.Type == TokenType.Break || Now.Type == TokenType.Parenthesis))
						{
							instruc.Instruction.Add(Now);
							Now = Input[++Current];
						}
						if (Now.Type == TokenType.Break)
						{
							Current++;
						}
						STokens.Add (instruc);
						continue;
					}

				}
				if (Now.Type == TokenType.PreProcessor)
				{
					STokens.Add(new SToken { Location = Now.Location, Type = STokenType.PreProcessor, Value = Now.Value });
					Current++;
					continue;
				}
				if (Now.Type == TokenType.Break)
				{
					Current++;
					continue;
				}
				Res.Errors.Add(new Error { Code = 2, Value = "Unexpected Token (" + Now.Type + ":" + Now.Value + ")", Location = Now.Location });
				break;
			}
		}

		/// <summary>
		/// Get the result.
		/// </summary>
		public SimplifierResult get()
		{
		//	foreach (SToken s in STokens) {
		//		System.Console.WriteLine ("[{0}:{1}]{2}",s.Location,s.Type,s.Value);
		//		if (s.Instruction?.Count > 0) {
		//			foreach (Token i in s.Instruction) {
		//				System.Console.WriteLine ("+[{0}:{1}]{2}", i.Location,i.Type,i.Value);
		//			}
		//		}
		//	}
			return new SimplifierResult
			{
				STokens = STokens,
				Errors = Res.Errors,
				Warnings = Res.Warnings,
			};
		}
	}
}
