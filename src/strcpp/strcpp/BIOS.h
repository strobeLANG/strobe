#pragma once
#include "Common.h"
#define BufferSize 128
namespace str
{
	namespace firmware
	{
		// Basic Output/Input System
		class BIOS
		{
		public:
			// Initialize the bios
			BIOS();

			// Write char array
			void Write(char*);

			// Write byte array
			void Write(BArray);

			// Display buffer to console
			void Display();
			
			// Write character
			void Write(char);

		private:
			// The buffer
			char buffer[BufferSize];
			// The locaton
			int loc;
		};
	}
}