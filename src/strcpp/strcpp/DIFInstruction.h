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

			// Initialize DIF Instruction
			DIFInstruction(OpType iop)
			{
				Operation = iop;
			}

			// Convert to normal instruction
			Instruction toInstruction()
			{
				return Instruction(Operation,Param);
			}
		};

		// DIF Instruction array
		typedef struct {
			DIFInstruction* value;
			int size;
		} IDifArray;
	}
}