using System.Collections.Generic;
namespace Strobe
{
	/// <summary>
	/// Lexer.
	/// </summary>
	public class Lexer
	{
		/// <summary>
		/// The result.
		/// </summary>
		Result Res;

		/// <summary>
		/// The current location.
		/// </summary>
		int Current;

		/// <summary>
		/// The tokens.
		/// </summary>
		List<Token> Tokens;

		/// <summary>
		/// The input.
		/// </summary>
		string Input;

		/// <summary>
		/// Initializes a new instance of the <see cref="Strobe.Lexer"/> class.
		/// </summary>
		/// <param name="input">Input.</param>
		public Lexer(string input)
		{
			// Create a new empty list of errors
			Res = new Result();
			// Set the current location to 0
			Current = 0;
			// Set the input to the argument
			Input = input;
			// Create a new empty list of tokens
			Tokens = new List<Token>();
			// Analyze
			Analyze();
		}

		/// <summary>
		/// Analyze this instance.
		/// </summary>
		void Analyze()
		{
			// Read while the current location is less than the lenght of the file
			while (Current < Input.Length)
			{
				// Set the current character
				char Now = Input[Current];

				// Check for pre-processor instructions
				if (Now == '#')
				{
					string pp = "";
					Now = Input[++Current];
					while (Now != System.Environment.NewLine[0])
					{
						pp += Now;
						Now = Input[++Current];
					}
					Tokens.Add(new Token { Value = pp, Type = TokenType.PreProcessor, Location = Current });
					continue;
				}

				// Parenthesis aka ()[]{}
				if (isParenthesis(Now))
				{
					// Add the token and continue with the next character
					Tokens.Add(new Token { Value = Now.ToString(), Type = TokenType.Parenthesis, Location = Current });
					Current++;
					continue;
				}

				// Ignore the whitespace (because it's useless)
				if (char.IsWhiteSpace(Now))
				{
					// Move on
					Current++;
					continue;
				}
				// Numbers
				if (isNumber(Now))
				{
					// Put the stuff in num
					string num = "";
					// While it is a number add it to num
					while (isNumber(Now) || isNumOp(Now))
					{
						num += Now;
						Now = Input[++Current];
					}
					// Add the token and move on
					Tokens.Add(new Token { Value = num, Type = TokenType.Number, Location = Current });
					continue;
				}
				// Strings
				if (isString(Now))
				{
					// Add the string to str
					string str = "";
					// Take out the "
					Now = Input[++Current];
					// Wait for a " and add everything else to the string
					while (!isString(Now))
					{
						// Escape character
						if (Now == '\\')
						{
							Now = Input[++Current];
							switch (Now)
							{
								case 'n':
									str += '\n';
									break;
								case 'r':
									str += '\r';
									break;
								case 't':
									str += '\t';
									break;
								case 'b':
									str += '\b';
									break;
								case 'f':
									str += '\f';
									break;
								case 'v':
									str += '\v';
									break;
								case '0':
									str += (char)0;
									break;
								default:
									str += Now;
									break;
							}
							Now = Input[++Current];
							continue;
						}
						str += Now;
						Now = Input[++Current];
					}
					// Also take out the " at the end, and move on
					Now = Input[++Current];
					Tokens.Add(new Token { Value = str, Type = TokenType.String, Location = Current });
					continue;
				}
				// I'm a compiler, why should i care about comments
				if (Now == '/' && Input[Current + 1] == '*')
				{
					Current++;
					Now = Input[++Current];
					bool Done = false;
					while (!Done)
					{
						Now = Input[++Current];
						if (Now == '*')
						{
							Now = Input[++Current];
							if (Now == '/')
							{
								break;
							}
						}
					}
					Now = Input[++Current];
					continue;
				}
				if (Now == '/' && Input[Current + 1] == '/')
				{
					Current++;
					Now = Input[++Current];
					while (Now != System.Environment.NewLine[0])
					{
						Now = Input[++Current];
					}
					continue;

				}
				// Check if it is a variable
				if (isVariable(Now))
				{
					TokenType type;
					if (Now == '$')
					{
						type = TokenType.Variable;
					}
					else {
						type = TokenType.Register;
					}
					// Put the variable name into vnam
					string vnam = "";
					// Cut the $ at the beginning
					Now = Input[++Current];
					// For better expirience
					while (isIdentifier(Now) || char.IsNumber(Now) || Now == '.')
					{
						vnam += Now;
						Now = Input[++Current];
					}
					// Add the tokens and move on
					Tokens.Add(new Token { Value = vnam, Type = type, Location = Current });
					continue;
				}
				// The identifier
				if (isIdentifier(Now))
				{
					string nam = "";
					while (isIdentifier(Now) || char.IsNumber(Now) || Now == '.')
					{
						nam += Now;
						Now = Input[++Current];
					}
					// Add the tokens and move on
					Tokens.Add(new Token { Value = nam, Type = TokenType.Identifier, Location = Current });
					continue;
				}
				// The operators (if it is not a valid operator, i don't care the parser will take care of it)
				if (isOperator(Now))
				{
					// Just put the operator in op
					string op = "";
					while (isOperator(Now))
					{
						op += Now;
						Now = Input[++Current];
					}
					// Add the tokens and move on
					Tokens.Add(new Token { Value = op, Type = TokenType.Operator, Location = Current });
					continue;
				}
				// If it is a ; add it to tokens (simple)
				if (isBreak(Now))
				{
					Tokens.Add(new Token { Value = ";", Type = TokenType.Break, Location = Current });
					Current++;
					continue;
				}
				// Too bad, you typed in some stuff i can't understand
				Res.Errors.Add(new Error { Value = "Unknown Character " + Now, Code = 1, Location = Current });
				// Let me take a break
				break;
			}
		}

		/// <summary>
		/// Is the character a number operator?
		/// </summary>
		/// <returns><c>true</c>, if it's a number operator, <c>false</c> otherwise.</returns>
		/// <param name="Char">Character.</param>
		bool isNumOp(char Char)
		{
			return Char == '.' || Char == 'x' || Char == '+'
				|| Char == '-' || Char == '*' || Char == '/'
				|| Char == '%';
		}

		/// <summary>
		/// Checks if the char is an operator.
		/// </summary>
		/// <returns><c>true</c>, if character is an operator, <c>false</c> otherwise.</returns>
		/// <param name="Char">Character.</param>
		bool isOperator(char Char)
		{
			return Char == '=' || Char == '!' || Char == '<' 
				|| Char == '>' || Char == '*' || Char == '/'
				|| Char == '+' || Char == '-' || Char == '%'
				|| Char == '|' || Char == '&' || Char == '~'
				|| Char == ',' || Char == ':' || Char == '.';
		}

		/// <summary>
		/// Checks if the caracter is a break.
		/// </summary>
		/// <returns><c>true</c>, if the character is a break, <c>false</c> otherwise.</returns>
		/// <param name="Char">Character.</param>
		bool isBreak(char Char)
		{
			return Char == ';';
		}

		/// <summary>
		/// Checks if the character is a parenthesis.
		/// </summary>
		/// <returns><c>true</c>, if character is parenthesis, <c>false</c> otherwise.</returns>
		/// <param name="Char">Character.</param>
		bool isParenthesis(char Char)
		{
			return Char == '[' || Char == ']' || Char == '(' || Char == ')' || Char == '{' || Char == '}';
		}

		/// <summary>
		/// Checks if the character is a number.
		/// </summary>
		/// <returns><c>true</c>, if character is a number, <c>false</c> otherwise.</returns>
		/// <param name="Char">Character.</param>
		bool isNumber(char Char)
		{
			return Char == '0' || Char == '1' || Char == '2'
				|| Char == '3' || Char == '4' || Char == '5'
				|| Char == '6' || Char == '7' || Char == '8'
				|| Char == '9';
		}

		/// <summary>
		/// Checks if the character is a string.
		/// </summary>
		/// <returns><c>true</c>, if character is string, <c>false</c> otherwise.</returns>
		/// <param name="Char">Character.</param>
		bool isString(char Char)
		{
			return Char == '"' || Char == '\'';
		}

		/// <summary>
		/// Checks if character is a variable.
		/// </summary>
		/// <returns><c>true</c>, if character is a variable, <c>false</c> otherwise.</returns>
		/// <param name="Char">Character.</param>
		bool isVariable(char Char)
		{
			return Char == '$' || Char == '@';
		}

		/// <summary>
		/// Checks if the character is an identifier.
		/// </summary>
		/// <returns><c>true</c>, if character is an identifier, <c>false</c> otherwise.</returns>
		/// <param name="Char">Char.</param>
		bool isIdentifier(char Char)
		{
			return char.IsLetter(Char) || Char == '_';
		}

		/// <summary>
		/// Get the result.
		/// </summary>
		public LexerResult get()
		{
			//foreach (Token t in Tokens)
			//{
			//	System.Console.WriteLine("[{0}]:{1}",t.Type,t.Value);
			//}
			return new LexerResult
			{
				Tokens = Tokens,
				Errors = Res.Errors,
				Warnings = Res.Warnings
			};
		}
	}
}
