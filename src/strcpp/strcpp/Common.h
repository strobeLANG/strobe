#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#pragma once
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

	// Arguments Parser
	IntArray ParseArgs(BArray);

	// Get second argument
	BArray GetSecond(BArray);
}