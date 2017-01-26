#include "Common.h"
namespace str
{

	// Read a file into byte array
	BArray ReadAllBytes(const char *name)
	{
		// The byte array to return.
		BArray ret;

		// The file stream to open
		std::ifstream infile(name);

		// Check the size
		infile.seekg(0, infile.end);
		ret.size = infile.tellg();
		infile.seekg(0, infile.beg);

		// Create the buffer
		ret.value = new byte[ret.size];

		// Read the file
		infile.read((char*)ret.value, ret.size);

		// Return the buffer
		return ret;
	}

	// Turn integer into 4 bytes
	BArray BArray4(int input)
	{
		// Define the byte array
		BArray ret;
		ret.size = 4;
		ret.value = new byte[ret.size];
		
		// Turn it into byte array
		ret.value[0] = (input >> 24) & 0xFF;
		ret.value[1] = (input >> 16) & 0xFF;
		ret.value[2] = (input >> 8) & 0xFF;
		ret.value[3] = input & 0xFF;

		// Return
		return ret;
	}

	// Get second argument
	BArray GetSecond(BArray input)
	{
		// Define the first return
		BArray ret;
		ret.size = input.size;
		ret.value = new byte[ret.size];

		// The new size
		int NewSize = 0;

		// Should add
		bool ShouldAdd = false;

		// Get the value
		for (int i = 0; i < ret.size; i++)
		{
			// Check if should add
			if (ShouldAdd)
				// Add
				ret.value[NewSize++] = ret.value[i];
			else
				// Check if byte is seperator character
				if (input.value[i] == 0xfe)
					ShouldAdd = true;
		}

		// Change the size
		ret.size = NewSize;

		// Change the value
		byte *NewValue = new byte[ret.size];

		// Copy the old value
		for (int i = 0; i < ret.size; i++)
			NewValue[i] = ret.value[i];

		// Does this create a memory leak?
		ret.value = NewValue;

		// Return
		return ret;
	}

	// Bytes to integer
	int Int32(BArray input)
	{
		// Error check
		if (input.size != 4)
			throw("To create a 32-bit integer you need 4 bytes.");

		// Integer to return.
		int ret = 0;

		// Calculate it
		for (int i = 0; i < 4; i++)
			ret = (ret << 8) + input.value[i];

		// Return
		return ret;
	}

	// Arguments parser
	IntArray ParseArgs(BArray input)
	{
		// Define the integer array
		IntArray ret;
		ret.size = 2;
		ret.value = new int[2];

		// Define the first byte array
		BArray first;
		first.value = new byte[4]{ 0,0,0,0 };
		first.size = 4;

		// Define the second byte array
		BArray second;
		second.value = new byte[4]{ 0,0,0,0 };
		second.size = 4;


		bool isFirst = true; // Is it the first byte array

		int vl = 0; // Variable location

		// Parse the arguments
		for (int i = 0; i < input.size; i++)
		{
			// Check if should write to the first byte array
			if (isFirst)
			{
				// If it is the seperator chracter, change to second
				if (input.value[i] == 0xfe)
				{
					isFirst = false;
					vl = 0;
				}
				else
				{
					// Make sure that it is 4 bytes
					if (vl < 4)
					{
						// Add to first variable
						first.value[vl] = input.value[i];
						vl++;
					}
				}
			}
			else
			{
				// Make sure that it is 4 bytes
				if (vl < 4)
				{
					// Add to second variable
					second.value[vl] = input.value[i];
					vl++;
				}
			}
		}

		// Turn the byte arrays into integers
		ret.value[0] = Int32(first);
		ret.value[1] = Int32(second);

		// Return the integers
		return ret;
	}
}