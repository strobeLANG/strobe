#include "Common.h"
#define MaxVars 1024
#pragma once
namespace str
{
	namespace runtime
	{
		// The Memory Manager Class
		class Memory
		{
		public:
			// Initialize
			Memory(int);

			// Allocate Space for a new variable
			void Allocate(int, int);

			// Put the data inside of the variable
			void Assign(int, BArray);

			// Clear the memory
			void Clear();

			// Clear the variable
			void Clear(int);

			// Get the memory
			BArray Get();

		private:
			// Size of memory
			int Size;
			// Variables
			Variable Variables[MaxVars];
			// Storage
			byte *Storage;
			// Current Address
			int CurrentAddress;
		};

		// Single Variable Structure
		typedef struct
		{
			// The size
			int Size = 0;
			// The address
			int Address = 0;
		} Variable;
	}
}