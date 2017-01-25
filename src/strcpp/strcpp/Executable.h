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
		virtual Instruction* CPU()
		{
			// Return the 
			return new Instruction[]{Instruction(Null,0)};
		}
	};

	// Execution types
	enum ExecType
	{
		DIF, // Direct Instruction Format
	};
}