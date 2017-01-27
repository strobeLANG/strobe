#include "Runtime.h"
#include "Processor.h"
namespace str
{
	namespace runtime
	{
		// Processor Definition
		BArray Runtime::Processor(Instruction current, int *args)
		{
			// Check for the operation type
			switch (current.Operation)
			{
			case Compare:
				break;

			case Interrupt:
				break;

			case Subtract:
				break;

			case Mutiply:
				break;
			case Add:
				break;

			case Divide:
				break;

			default:
				throw("Invalid operation type.");
			}



			BArray *temp = 0;
			return *temp;
		}
	}
}