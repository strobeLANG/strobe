using System.Collections.Generic;
namespace Strobe
{
	// The parse tree is bunch of namespaces.
	public class ParseTree : Data
	{
		public List<Namespace> Namespaces;
		public List<string> Preprocessor;
	}

	// A namespace is bunch of functions.
	public class Namespace : Data
	{
		public string Name;
		public List<Function> Functions;
	}

	// A function is bunch of Instructions that return something.
	public class Function : Data
	{
		public string Name;
		public Args Arguments;
		public List<Instruction> Instructions;
		public Return Ret;
	}
	// A return is a type and a value
	public class Return
	{
		public TokenType Type;
		public string Value;
	}

	// An Instruction is a function, an operator and a variable.

	public class Instruction : Data
	{
		public InstructionType Type;
		public Variable Var;
		public Operator Op;
		public Execute Func;
	}
	// A variable has a name and value
	public class Variable : Data
	{
		public bool ignore = false;
		public bool isNum = false;
		public bool isConst = false;
		public bool isReg = false;
		public string Name;
	}
	// An operator has a type
	public class Operator : Data
	{
		public string Type;
	}
	// A Execute (function call) has a namespace ID, function id and arguments.
	public class Execute : Data
	{
		public string Namespace;
		public string Function;
		public Args Arguments;
	}
	// The arguments have variables.
	public class Args : Data
	{
		public List<Variable> Arguments;
	}

	// Location is pointless because it won't be accurate
	public class Data
	{
	}
	// The instruction types
	public enum InstructionType
	{
		// First
		Void = -1,				// ;
		VoidFunc = 0,			// Execute;
		VarFunc	= 2,			// $Var = Execute;
		// Second
		VarFuncArg = 3,			// $Var = Execute(Arg);
		VoidFuncArg = 4,		// Execute(Arg);
		VoidArg = -2,			// (Arg);
	}
}
