using System.Collections.Generic;
namespace Strobe
{
	// I'm Token's "slow" cousin
	public class SToken
	{
		// The type.
		public STokenType Type;
		// The value.
		public string Value;
		// The location.
		public int Location;
		// The id (this is used to make stuff easier) <- nope
		//public int Id;
		// This is an instruction
		public List<Token> Instruction;

	}

	// The Simpler Token Types
	public enum STokenType
	{
		// All the simpler types
		Namespace = 0,
		Instruction = 1,
		Function = 2,
		Block = 3,
		Return = 4,
		PreProcessor = 6,
		Arguments = 7
	}
}
