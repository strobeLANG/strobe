#include "Common.h"
#include "Executable.h"
#pragma once
namespace str
{
	// Format abstract class
	class Format
	{
	public:
		// Abastract Load
		virtual Executable Load(byte *Input) = 0;
		// Abstract GetBytes
		virtual byte* GetBytes(Executable Input) = 0;
	};
}