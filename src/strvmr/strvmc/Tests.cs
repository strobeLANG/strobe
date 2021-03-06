﻿namespace strvmc
{
	/// <summary>
	/// Tests.
	/// </summary>
	public static class Tests
	{

		/// <summary>
		/// Hellos the world.
		/// </summary>
		/// <returns>The program.</returns>
		public static byte[] HelloWorld()
		{
			return new byte[]
			{
				0x0,0x4,0x1c,0xfe,0xc,0xff,				// define 0x1c with size 0xc
				0x0,0x5,0x1c,0xfe,						// add data to 0x1c
				0x48,0x65,0x6c,0x6c,0x6f,0x20,			// "Hello "
				0x77,0x6f,0x72,0x6c,0x64,0xa,0xff, 		// "World\n"
				0x0,0x9,0x1,0xfe,0x1c,0xff,				// put the address of 0x1c in 0x1
				0x0,0xa,0x1,0xff,						// define the label 0x1
				0x0,0x6,0x1,0xff,						// bios write to buffer (takes 0x1)
				0x0,0x6,0x2,0xff,						// bios display buffer
				0x0,0x5,0x2,0xfe,0x0,0xff,				// make 0x2 be 0x0
				0x0,0xb,0x1,0x2,0xff,					// goto label 0x1 (if 0x2 is 0x0)
				0x0,0x5,0x1,0xfe,0x0,0xff,				// make 0x1 be 0x0
				0x0,0x6,0x0,0xff						// bios exit with error code (takes 0x1)
			};
		}

		/// <summary>
		/// Reads the key and prints it.
		/// </summary>
		/// <returns>The program.</returns>
		public static byte[] KeyRead()
		{
			return new byte[]
			{
				0x0,0x4,0x1c,0xfe,0x50,0xff,			// define 0x1c with size 0xfa+0xfa
				0x0,0x9,0x0,0xfe,0x1c,0xff,				// put the address of 0x1c in 0x0
				0x0,0xa,0x1,0xff,						// define the label 0x1
				0x0,0xc,0x1,0xff,						// clear 0x1
				0x0,0x5,0x1,0xfe,0x3e,0x3e,0xff,		// make 0x1 be >>
				0x0,0x6,0x1,0xff,						// bios write to buffer (takes 0x1)
				0x0,0x6,0x2,0xff,						// bios display buffer
				0x0,0x6,0x4,0xff,						// ask the user for his character
				0x0,0x9,0x1,0xfe,0x0,0xff,				// print his character on the screen
				0x0,0x6,0x1,0xff,						// bios write to buffer (takes 0x1)
				0x0,0x6,0x2,0xff,						// bios display buffer
				0x0,0xc,0x1,0xff,						// clear 0x1
				0x0,0x5,0x1,0xfe,0xa,0xff,				// make 0x1 be 0xa
				0x0,0x6,0x1,0xff,						// bios write to buffer (takes 0x1)
				0x0,0x6,0x2,0xff,						// bios display buffer
				0x0,0xc,0x0,0xff,						// clear 0x0
				0x0,0xb,0x1,0xc,0xff					// goto label 0x1 (if 0x2 is 0x0) 
			};
		}
	}
}
