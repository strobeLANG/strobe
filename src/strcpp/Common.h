#pragma once
#include <vector>
#include <iostream>
#include <fstream>
#include <algorithm>
#include <iterator>

using namespace std;
namespace str
{
	// unsigned char = byte
	typedef unsigned char Byte;
	

	// ReadAllBytes
	vector<Byte> ReadAllBytes(const char *name)
	{
		// Found this on the internet, hope it works.
		// And also good start, copying from the internet.

		typedef istream_iterator<Byte> istream_iterator;
		ifstream file(name);
		vector<Byte> input;

		file >> noskipws;

		copy(istream_iterator(file), istream_iterator(),
			back_inserter(input));

		return input;

	}

	// Turn integer into 4 bytes
	vector<Byte> Int2Byt(int input)
	{
		// Define the byte vector
		vector<Byte> ret;
		ret.resize(4);

		// Turn it into byte vector
		ret[0] = (input >> 24) & 0xFF;
		ret[1] = (input >> 16) & 0xFF;
		ret[2] = (input >> 8) & 0xFF;
		ret[3] = input & 0xFF;

		// Return
		return ret;
	}

	// Get second argument
	vector<Byte> GetSecond(vector<Byte> input)
	{
		// Define the first return
		vector<Byte> ret;
		// Should add
		bool ShouldAdd = false;

		// Get the value
		for (int i = 0; (size_t)i < ret.size(); i++)
		{
			// Check if should add
			if (ShouldAdd)
				// Add
				ret.push_back(ret[i]);
			else
				// Check if byte is seperator character
				if (input[i] == 0xfe)
					ShouldAdd = true;
		}
		// Return
		return ret;
	}

	// Bytes to integer
	int Byt2Int(vector<Byte> input)
	{
		// Error check
		if (input.size() != 4)
			throw("To create a 32-bit integer you need 4 bytes.");

		// Integer to return.
		int ret = 0;

		// Calculate it
		for (int i = 0; i < 4; i++)
			ret = (ret << 8) + input[i];

		// Return
		return ret;
	}

	// Arguments parser
	vector<int> ParseArgs(vector<Byte> input)
	{
		// Define the integer vector
		vector<int> ret;
		ret.resize(2);

		// Define the first byte vector
		vector<Byte> first;
		first.resize(4);
		first = { 0,0,0,0 };

		// Define the second byte array
		vector<Byte> second;
		second.resize(4);
		second = { 0,0,0,0 };


		bool isFirst = true; // Is it the first byte array

		int vl = 0; // Variable location

					// Parse the arguments
		for (int i = 0; (size_t)i < input.size(); i++)
		{
			// Check if should write to the first byte array
			if (isFirst)
			{
				// If it is the seperator chracter, change to second
				if (input[i] == 0xfe)
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
						first[vl] = input[i];
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
					second[vl] = input[i];
					vl++;
				}
			}
		}

		// Turn the byte arrays into integers
		ret[0] = Byt2Int(first);
		ret[1] = Byt2Int(second);

		// Return the integers
		return ret;
	}

}