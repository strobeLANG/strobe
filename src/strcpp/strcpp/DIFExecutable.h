#pragma once
#include "Executable.h"
#include "DIFInstruction.h"
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
			IArray CPU();

			// Add an instruction
			void AddInst(DIFInstruction);

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