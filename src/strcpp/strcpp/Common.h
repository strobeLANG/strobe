#pragma once
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
namespace str
{
	// Byte
	typedef unsigned char byte;

	// Byte Array
	typedef struct {
		byte* value;
		int size;
	} BArray;

	// Int Array
	typedef struct {
		int *value;
		int size;
	} IntArray;


	// Byte array into integer
	int Int32(BArray);

	// Integer into byte array
	BArray BArray4(int);

	// Arguments Parser
	IntArray ParseArgs(BArray);

	// Get second argument
	BArray GetSecond(BArray);
}