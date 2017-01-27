#pragma once
#include <stdio.h>
#include <fstream>
#include <iostream> 
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

	// Read a file into byte array
	BArray ReadAllBytes(const char*);

	// Byte array into integer
	int Int32(BArray);

	// Integer into byte array
	BArray BArray4(int);

	// Arguments Parser
	IntArray ParseArgs(BArray);

	// Get second argument
	BArray GetSecond(BArray);
}