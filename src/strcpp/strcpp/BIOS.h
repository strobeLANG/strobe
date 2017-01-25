#include "Common.h"
#include <stdio.h>
#include <string.h>
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
			// Return the code of BIOS Setup
			static byte* Setup()
			{
				return new byte[4]
				{
					// Currently not implemented,
					// return the instruction for exit
					0x0,0x6,0x0,0xff
				};
			}
			// Initialize the bios
			BIOS()
			{
				loc = 0;
			}

			// Write char array
			void Write(char* c)
			{
				Write((byte*)c);
			}

			// Write byte array
			void Write(byte* c)
			{
				for (int i = 0; i < strlen((char *)c); i++)
				{
					Write((char)c[i]);
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