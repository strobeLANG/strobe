#include "Common.h"
#define BufferSize 128
#pragma once
namespace str
{
	namespace firmware
	{
		// Basic Output/Input System
		class BIOS
		{
		public:
			// Initialize the bios
			BIOS()
			{
				loc = 0;
			}

			// Write char array
			void Write(char* c)
			{
				BArray i;
				i.value = (byte *)c;
				i.size = strlen(c);
				Write(i);
			}

			// Write byte array
			void Write(BArray c)
			{
				for (int i = 0; i < c.size; i++)
				{
					Write((char)c.value[i]);
				}
			}

			// Display buffer to console
			void Display()
			{
				loc = 0;
				// Print every character
				for(int i = 0; i < BufferSize; i++)
					if (buffer[i] > 5) printf("%c",buffer[i]);
			}
			
			// Write character
			void Write(char c)
			{
				if (loc < BufferSize)
				{
					buffer[loc++] = c;
				}
				else
				{
					Display();
					buffer[loc++] = c;
				}
			}

		private:
			// The buffer
			char buffer[BufferSize];
			// The locaton
			int loc;
		};
	}
}