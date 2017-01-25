#include "Runtime.h"

namespace str
{
	namespace runtime
	{
		// Work definition
		void Runtime::Work(Instruction current)
		{
			switch (current.Operation)
			{
			case Assign:
				break;
			case Addr:
				break;
			case Move:
				break;
			case Allocate:
				break;
			case Label:
				break;
			case Goto:
				break;
			case Clear:
				break;
			default:
				break;
			}
		}

		// Reserved definition
		void Runtime::Reserved(int num)
		{
			// 4 Zeros (empty integer)
			BArray z4;
			z4.size = 4;
			z4.value = new byte[4]{0,0,0,0};

			// Allocate and fill with 0's
			for (int i = 0; i < num; i++)
			{
				memory->Allocate(i, 4);
				memory->Assign(i,z4);
			}
		}
	}
}