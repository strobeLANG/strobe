#include "Runtime.h"

namespace str
{
	namespace runtime
	{
		// Running definition
		bool Runtime::Running()
		{
			// Check the running processes.
			for (int i = 0; i < processes->current; i++)
				// If a process is running.
				if (processes->value[i].running)
					// Return true
					return true;
			// If no processes are running, return false.
			return false;
		}

		// Runtime definition
		Runtime::Runtime(int Size)
		{
			int pSize = (Size / 1024) + 1;
			// Initialize memory
			memory = new Memory(Size);

			// Initialize processes
			processes->size = pSize;
			processes->value = new Process[processes->size];
			processes->current = 0;

			// Initialize labels
			labels->size = pSize * 64;
			labels->value = new cLabel[labels->size];
			labels->current = 0;

			// Initialize reserved
			Reserved(16);
		}

		//  Start Definition
		void Runtime::Start(Executable iExec)
		{
			// Do a check
			if (processes->current >= processes->size)
				throw("Too many processes running at the same time.");

			// Create the process
			Process newProcess;
			newProcess.inst = iExec.CPU();
			newProcess.current = 0;
			newProcess.running = true;

			// Add the process
			processes->value[processes->current] = newProcess;

			// Increase the process counter counter
			processes->current++;
		}

		// Step definition
		void Runtime::Step()
		{
			// For every process
			for (int i = 0; i < processes->current; i++)
				// If the process is running, step.
				if (processes->value[i].running)
				{
					pNum = i;
					// Try to step
					try
					{
						Work(processes->value[i].Step());
					}
					// Catch the boolean (and set it as the current running status)
					catch (bool status)
					{
						processes->value[i].running = status;
					}
				}
		}
	}
}
