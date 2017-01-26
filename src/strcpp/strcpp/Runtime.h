#pragma once
#include "Memory.h"
#include "DIFExecutable.h"
#include "BIOS.h"
using namespace str::dif;
using namespace str::firmware;
namespace str
{
	namespace runtime
	{

		// Label
		typedef struct
		{
			int ID;
			int Location;
		} cLabel;

		// Label Array
		typedef struct
		{
			cLabel *value;
			int size;
			int current;
		} LArray;

		// Process
		typedef struct
		{
			IArray inst;
			bool running;
			int current;

			// Step in the process
			Instruction Step()
			{
				if (current >= inst.size)
				{
					throw(false);
				}
				current++;
				return inst.value[current];
			}

		} Process;

		// Process Array
		typedef struct
		{
			Process *value;
			int size;
			int current;
		} PArray;
		// The runtime class
		class Runtime
		{
		public:
			// Running check
			bool Running();

			// Initialize the runtime
			Runtime(int);

			//  Add a new process
			void Start(Executable);

			// Step once in every process
			void Step();

			// Basic output / input system (it's public)
			BIOS bios;

		private:
			// Current Process Number
			int pNum;

			// Process the instruction
			void Work(Instruction);

			// Initialize reserved
			void Reserved(int);

			// The memory
			Memory *memory;

			// Processes
			PArray *processes;

			// Labels
			LArray *labels;

			// Processor
			BArray Processor(Instruction, int*);
		};
	}
}