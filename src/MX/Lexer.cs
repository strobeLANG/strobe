using System.Collections.Generic;
namespace MX
{
	// The Lexer Class
	public class Lexer
	{
		// The lexer errors
		Result Res;
		// The current location
		int Current;
		// The current tokens
		List<Token> Tokens;
		// The input
		string Input;

		// Define the lexer and call the analyze
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

		// The Lexer
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
					while (isNumber(Now) || isOperator(Now))
					{
						num += Now;
						Now = Input[++Current];
					}
					// Add the token and move on
					Tokens.Add(new Token { Value = num, Type = TokenType.Number, Location = Current });
					Current++;
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

		// The Operators
		bool isOperator(char Char)
		{
			return Char == '=' || Char == '!' || Char == '<' 
				|| Char == '>' || Char == '*' || Char == '/'
				|| Char == '+' || Char == '-' || Char == '%'
				|| Char == '|' || Char == '&' || Char == '~'
				|| Char == ',' || Char == ':' || Char == '.';
		}
		// The Breaks
		bool isBreak(char Char)
		{
			return Char == ';';
		}
		// The Parenthesis
		bool isParenthesis(char Char)
		{
			return Char == '[' || Char == ']' || Char == '(' || Char == ')' || Char == '{' || Char == '}';
		}
		// The Numbers
		bool isNumber(char Char)
		{
			return Char == '0' || Char == '1' || Char == '2'
				|| Char == '3' || Char == '4' || Char == '5'
				|| Char == '6' || Char == '7' || Char == '8'
				|| Char == '9';
		}
		// The Strings
		bool isString(char Char)
		{
			return Char == '"' || Char == '\'';
		}
		// The Variables
		bool isVariable(char Char)
		{
			return Char == '$' || Char == '@';
		}
		// The Identifiers
		bool isIdentifier(char Char)
		{
			return char.IsLetter(Char) || Char == '_';
		}
		// Return the result with all of the errors
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
