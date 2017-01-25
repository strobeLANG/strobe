#include "Memory.h"

namespace str
{
	namespace runtime
	{
		// Get definition
		BArray Memory::Get()
		{
			// Define the byte array
			BArray ret;

			// Set the correct size and value
			ret.size = Size;
			ret.value = Storage;

			// Return it
			return ret;
		}

		// Assign definition
		void Memory::Assign(int iLoc, BArray ibytes)
		{
			// Do error checks
			if (iLoc >= MaxVars)
				throw("Variable is impossible to be declared.");
			if (Variables[iLoc].Size == 0)
				throw("Variable is not declared.");

			// Put inside the memory
			for (int i = Variables[iLoc].Address; i < Variables[iLoc].Address + Variables[iLoc].Size; i++)
				Storage[i] = ibytes.value[i - Variables[iLoc].Address];
		}

		// Memory Definition
		Memory::Memory(int iSize)
		{
			Size = iSize;
			Storage = (byte*)malloc(Size);
		}

		// Allocate definition
		void Memory::Allocate(int iLoc, int iSize)
		{
			// Do error checks
			if (iLoc >= MaxVars)
				throw("Too many variables defined, please change MaxVars in the source.");
			if (iSize + CurrentAddress >= Size)
				throw("Out of virtual memory, please increase the virtual memory in the runtime.");

			// Create a new variable
			Variable var;
			var.Address = CurrentAddress;
			var.Size = iSize;

			// Move the current address
			CurrentAddress += iSize;

			// Add the variable to the variables list
			Variables[iLoc] = var;
		}

		// Clear definition
		void Memory::Clear()
		{
			// Replace everything with 0's
			for (int i = 0; i < Size; i++)
				Storage[i] = 0;
		}

		// Clear definition
		void Memory::Clear(int iLoc)
		{
			// Do error checks
			if (iLoc >= MaxVars)
				throw("Variable is impossible to be declared.");
			if (Variables[iLoc].Size == 0)
				throw("Variable is not declared.");

			// Replace the memory with 0's
			for (int i = Variables[iLoc].Address; i < Variables[iLoc].Address + Variables[iLoc].Size; i++)
				Storage[i] = 0;
		}
	}
}