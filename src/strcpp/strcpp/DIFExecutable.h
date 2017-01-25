#include "Executable.h"
#include "DIFInstruction.h"
#pragma once
namespace str
{
	namespace dif
	{
		// The DIF executable
		class DIFExecutable : public Executable
		{
		public:
			// Instruction list
			IDifArray Instructions;
			// Turn it into a CPU Instruction Array
			IArray CPU()
			{
				Instruction* insts = (Instruction *)malloc(Instructions.size);
				for (int i = 0; i < Instructions.size; i++)
				{
					insts[i] = Instructions.value[i].toInstruction();
				}
				IArray ret;
				ret.size = Instructions.size;
				ret.value = insts;
				return ret;
			}

			// Add an instruction
			void AddInst(DIFInstruction ins)
			{
				if (_i < _size)
					Instructions.value[_i++] = ins;
				else
					throw;
			}

			// Define the executable
			DIFExecutable(int iSize) : Executable(DIF)
			{
				_i = 0;
				_size = iSize;
				Instructions.size = _size;
				Instructions.value = (DIFInstruction*)malloc(_size);
			}
		private:
			// Size
			int _size;
			// Location
			int _i;
		};
	}
}