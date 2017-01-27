#include "BIOS.h"
namespace str
{
	namespace firmware
	{
		// Define BIOS
		BIOS::BIOS()
		{
			loc = 0;
		}

		// Define Write
		void BIOS::Write(char* c)
		{
			BArray i;
			i.value = (byte *)c;
			i.size = strlen(c);
			Write(i);
		}

		// Define Write
		void BIOS::Write(BArray c)
		{
			for (int i = 0; i < c.size; i++)
			{
				Write((char)c.value[i]);
			}
		}

		// Define display
		void BIOS::Display()
		{
			loc = 0;
			// Print every character
			for (int i = 0; i < BufferSize; i++)
				if (buffer[i] > 5) printf("%c", buffer[i]);
		}

		// Define Write
		void BIOS::Write(char c)
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

	}
}