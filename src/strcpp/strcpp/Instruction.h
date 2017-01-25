#include "Common.h"
#pragma once
namespace str
{
	// The instruction class
	class Instruction
	{
	public:
		OpType Operation; // Operation
		BArray Parameters; // Parameters

		// Definition
		Instruction(OpType inop, BArray inp)
		{
			Operation = inop; // Operation
			Parameters = inp; // Parameters
		}
	};

	// Instruction Array
	typedef struct{
		Instruction *value;
		int size;
	} IArray;
	
	// Operation Type Enum
	enum OpType
	{
		Null, // Nothing.
		Add, // Add two numbers
		Subtract, // Subtract two numbers
		Divide, // Divide two numbers
		Mutiply, // Multiply two numbers
		Allocate, // Allocate space
		Assign, // Asign data to variable
		Interrupt, // Interrupt
		Compare, // Compare
		Move, // Move data from variable to variable
		Addr, // Move address from variable to variable
		Goto, // Jump to a label
		Label, // Define a label
		Clear, // Clear a variable
	};
}