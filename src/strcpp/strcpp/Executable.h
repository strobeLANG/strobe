#pragma once
#include "Instruction.h"
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
			return IArray();
		}
	};

	// Execution types
	enum ExecType
	{
		DIF, // Direct Instruction Format
	};
}