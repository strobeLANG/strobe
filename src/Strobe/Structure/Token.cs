using System.Collections.Generic;
namespace Strobe
{
	// The token class
	public class Token
	{
		// The value of the token.
		public string Value;

		// The type of the token.
		public TokenType Type;

		// Location
		public int Location;
	}

	// The Token Types
	public enum TokenType
	{
		// All the types
		Parenthesis = 0,
		Number = 1,
		String = 2,
		Identifier = 3,
		Break = 4,
		Variable = 5,
		Operator = 6,
		Register = 7,
		PreProcessor = 8,
	}

}
