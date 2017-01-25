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
}