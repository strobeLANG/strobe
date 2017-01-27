#pragma once
#include "Instruction.h"
namespace str
{
	namespace dif
	{
		// Dif Instruction
		class DIFInstruction
		{
		public:
			// The operation
			OpType Operation;
			// The parameters
			BArray Param;

			// Default initializer
			DIFInstruction()
			{
				
			}

			// Initialize DIF Instruction
			DIFInstruction(OpType iop)
			{
				Operation = iop;
			}

			// Convert to normal instruction
			Instruction toInstruction()
			{
				Instruction x = Instruction(Operation, Param);
				return x;
			}
		};

		// DIF Instruction array
		typedef struct {
			DIFInstruction* value;
			int size;
		} IDifArray;
	}
}