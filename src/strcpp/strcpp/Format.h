#include "Executable.h"
#pragma once
namespace str
{
	// Format abstract class
	class Format
	{
	public:
		// Abastract Load
		virtual Executable Load(BArray Input) = 0;
		// Abstract GetBytes
		virtual BArray GetBytes(Executable Input) = 0;
	};
}