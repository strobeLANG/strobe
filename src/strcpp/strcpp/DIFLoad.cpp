#include "DIFFormat.h"
namespace str
{
	namespace dif
	{
		// Loading Definition
		Executable DIFFormat::Load(BArray Input)
		{
			int difsize = 0;
			// Find how many instructions are there
			// Current algorithm sucks because it counts
			// How many insturction ending characters are there
			for (int i = 0; i < Input.size; i++)
				if (Input.value[i] == 255) difsize++;

			// Define the default values
			DIFExecutable Return = DIFExecutable(difsize);
			BArray *b; b->value = 0; b->size = 0;
			byte temp[TempSize];
			DIFInstruction *cInst = new DIFInstruction(Null);
			byte cNow; int cPos = 0; int iPos = 0;

			// Parse the file
			while (cPos < Input.size)
			{
				// Current byte
				cNow = Input.value[cPos];

				// Check the byte
				switch (cNow)
				{
					// Instruction begin or byte 0
				case 0:
					// If no instruction has been defined, it's a instruction begin
					if (cInst->Operation == Null)
					{
						// Create it, using the next character as OpType
						cInst = new DIFInstruction(OpTypeFromByte(Input.value[++cPos]));
					}
					else
					{
						// Add the bytes to parameters
						temp[b->size] = cNow;
						b->size++;
					}
					break;
					// End of the instruction.
				case 255:
					// Add the value.
					b->value = new byte[b->size];
					for (int i = 0; i < b->size; i++)
						b->value[i] = temp[i];
					// Add the parameters.
					cInst->Param.size = b->size;
					cInst->Param.value = b->value;

					// Reset the B.
					b->size = 0;
					b->value = 0;

					// Add the instruction to return.
					Return.AddInst(*cInst);

					// Create a new instruction.
					cInst = new DIFInstruction(Null);
					break;
					// A normal byte.
				default:
					// Add the bytes to parameters
					temp[b->size] = cNow;
					b->size++;
					break;
				}
				cPos++;
			}

			// Return the value
			return Return;
		}
	}
}