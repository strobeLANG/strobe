#include "Runtime.h"

namespace str
{
	namespace runtime
	{
		// Work definition
		void Runtime::Work(Instruction current)
		{
			// Parse the arguments (the size is 2)
			 int *args = ParseArgs(current.Parameters).value;

			// Check he operation
			switch (current.Operation)
			{
				// Assign
			case Assign:
				memory->Assign(args[0], GetSecond(current.Parameters));
				break;

				// Move Address and Size
			case Addr:
				memory->SetVariable(args[0], memory->GetVariable(args[1]));
				break;

				// Move Contents
			case Move:
				memory->Assign(args[0],memory->Get(args[1]));
				break;

				// Allocate
			case Allocate:
				memory->Allocate(args[0], args[1]);
				break;

				// Label
			case Label:
				cLabel x;
				x.ID = args[0];
				x.Location = processes->value[pNum].current;
				labels->value[labels->current++] = x;
				break;

				// Goto label
			case Goto:
				if (Int32(memory->Get(args[1])) == 0x0)
				for(int i = 0; i<labels->current; i++)
					if (labels->value[i].ID == args[0])
					{
						// Change the process location
						processes->value[pNum].current = labels->value[i].Location;
						return;
					}
				break;

				// Clear
			case Clear:
				memory->Clear(args[0]);
				break;
				// Execute the other commands
			default:
				memory->Assign(0,Processor(current,args));
				break;
			}
		}

		// Reserved definition
		void Runtime::Reserved(int num)
		{
			// Allocate
			for (int i = 0; i < num; i++)
				memory->Allocate(i, 4);
		}
	}
}