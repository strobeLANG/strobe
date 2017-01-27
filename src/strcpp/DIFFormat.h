#pragma once
#include "Format.h"
#include "DIFExecutable.h"
#define TempSize 1024
namespace str
{
	namespace dif
	{
		// The DIF Format
		class DIFFormat : public Format
		{
		public:
			// Turn the executable into bytes
			BArray GetBytes(Executable)
			{
				// Not implemented, might never implement, it's not needed.
				throw("Not implemented.");
			}

			// Load the executable
			Executable Load(BArray);

			// Turn the OpType into byte
			byte ByteFromOpType(OpType);

			// Turn the byte into OpType
			OpType OpTypeFromByte(byte);

		};
	}
}