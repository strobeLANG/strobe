#include "DIFExecutable.h"
namespace str
{
	namespace dif
	{
		// Define CPU
		IArray DIFExecutable::CPU()
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

		// Define AddInst
		void DIFExecutable::AddInst(DIFInstruction ins)
		{
			if (_i < _size)
				Instructions.value[_i++] = ins;
			else
				throw;
		}

	}
}