#pragma once
#include "Common.h"
#define MaxVars 1024
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
			
			// Get Variable
			Variable GetVariable(int);

			// Set Variable
			void SetVariable(int,Variable);

			// Get Contents
			BArray Get(int);

			// Get the memory
			BArray Get();

			// Get Range
			BArray GetRange(int,int);

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