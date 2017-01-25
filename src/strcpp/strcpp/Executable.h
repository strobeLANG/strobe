#include "Instruction.h"
#pragma once
namespace str
{
	// The executable class
	class Executable
	{
	public:
		// Current type
		ExecType Type;

		// Class Start
		Executable(ExecType et)
		{
			Type = et;
		}

		// Virtual Instruction Array CPU
		virtual IArray CPU()
		{
			// Define the array
			IArray ret;
			ret.size = 1;
			BArray by;
			by.value = new byte[1]{ 0 };
			by.size = 1;
			ret.value = new Instruction[]{ Instruction(Null, by) };
			// Return the IArray
			return ret;
		}
	};

	// Execution types
	enum ExecType
	{
		DIF, // Direct Instruction Format
	};
}