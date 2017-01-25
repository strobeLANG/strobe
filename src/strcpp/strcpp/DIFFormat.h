#include "Format.h"
#include "DIFExecutable.h"
#define TempSize 1024
#pragma once
namespace str
{
	namespace dif
	{
		// The DIF Format
		class DIFFormat : public Format
		{
			// Turn the executable into bytes
			BArray GetBytes(Executable Input)
			{
				// Not implemented, might never implement, it's not needed.
				throw("Not implemented.");
			}

			// Load the executable
			Executable Load(BArray Input)
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

			// Turn the OpType into byte
			byte ByteFromOpType(OpType OpType)
			{
				switch (OpType)
				{
				case Add:
					return 0;
				case Subtract:
					return 1;
				case Divide:
					return 2;
				case Mutiply:
					return 3;
				case Allocate:
					return 4;
				case Assign:
					return 5;
				case Interrupt:
					return 6;
				case Compare:
					return 7;
				case Move:
					return 8;
				case Addr:
					return 9;
				case Label:
					return 10;
				case Goto:
					return 11;
				case Clear:
					return 12;
				default:
					throw("Incorrect OpType: " + OpType);
				}
			}

			// Turn the byte into OpType
			OpType OpTypeFromByte(byte OpByte)
			{
				switch (OpByte)
				{
				case 0:
					return Add;
				case 1:
					return Subtract;
				case 2:
					return Divide;
				case 3:
					return Mutiply;
				case 4:
					return Allocate;
				case 5:
					return Assign;
				case 6:
					return Interrupt;
				case 7:
					return Compare;
				case 8:
					return Move;
				case 9:
					return Addr;
				case 10:
					return Label;
				case 11:
					return Goto;
				case 12:
					return Clear;
				default:
					throw("Incorrect OpByte: " + (int)OpByte);
				}
			}
		};
	}
}