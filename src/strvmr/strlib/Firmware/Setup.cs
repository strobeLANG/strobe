using System.Text;
using System.Collections.Generic;
using System.IO;

namespace StrobeVM.Firmware
{
    /// <summary>
    /// BIOS Setup.
    /// </summary>
    public static class Setup
	{
		const string Name = "BIOS Setup";
		const string Version = "v0.0.1";
		const byte Begin = 0x0;
		const byte End = 0xff;
		const byte Split = 0xfe;
		const byte Assign = 0x5;
		const byte Define = 0x4;
		const byte True = 0x1;
		const byte False = 0x0;
		const byte MovAddr = 0x9;
		const byte arg0 = 0x1;
		const byte ret = 0x0;
		const byte Call = 0x6;
		const byte Exit = 0x0;
		const byte Write = 0x1;
		const byte Show = 0x2;
		const byte Clear = 0xc;
		const byte ReadKey = 0x5;
		static byte next = 0x1c;

		/// <summary>
		/// Writes the top.
		/// </summary>
		/// <returns>The top.</returns>
		public static byte[] WriteTop()
		{
			List<byte> bytes = new List<byte> ();
			bytes = addBytes (bytes, ConsoleWrite (
				"=========================================================================\n" +
				"=\t\t\t"+Name+"\t"+Version+"\t\t\t\t=\n" +
				"=========================================================================\n"
			));
			return bytes.ToArray();
		}

		/// <summary>
		/// Writes the middle.
		/// </summary>
		/// <returns>The middle.</returns>
		public static byte[] WriteMid()
		{
			List<byte> bytes = new List<byte> ();
			bytes = addBytes (bytes, ConsoleWrite (
				"# Unable to load custom setup, make sure that \"setup.dif\" exists!"
			));
			bytes = addBytes (bytes, ConsoleWrite ("\n=========================================================================\n"));
			return bytes.ToArray();
		}

		/// <summary>
		/// Writes the bottom.
		/// </summary>
		/// <returns>The bottom.</returns>
		public static byte[] WriteBot()
		{
			List<byte> bytes = new List<byte> ();
			bytes = addBytes (bytes, ConsoleWrite (">> "));
			bytes = addBytes (bytes, ConsoleReadKey(0x0));
			bytes = addBytes (bytes, ConsoleWrite ("\n\n\n"));
			return bytes.ToArray();
		}

		/// <summary>
		/// Creates the menu.
		/// </summary>
		public static byte[] Menu()
		{
			List<byte> bytes = new List<byte> ();
			bytes = addBytes (bytes, WriteTop ());
			bytes = addBytes (bytes, WriteMid ());
			bytes = addBytes (bytes, WriteBot ());
			return bytes.ToArray ();
		}

		/// <summary>
		/// Gets the setup.
		/// </summary>
		/// <returns>The setup.</returns>
		public static byte[] GetSetup()
		{
			List<byte> setup = new List<byte>(); // Initialize the byte list

			// Check if custom bios setup is available
			if(File.Exists("setup.dif"))
			{
				// Load the custom bios setup
				setup = addBytes(setup,WriteTop());
				setup = addBytes(setup,File.ReadAllBytes("setup.dif"));
				setup = addBytes(setup,ProgramExit());
			}
			else 
			{
				// Create internal bios setup
				setup = addBytes (setup, Menu());
				setup = addBytes (setup, ProgramExit());
			}
			return setup.ToArray ();
		}

		/// <summary>
		/// Interrupt the CPU to exit.
		/// </summary>
		public static byte[] ProgramExit()
		{
			return new byte[] {
				Begin,Call,Exit,End
			};
		}

		/// <summary>
		/// Interrupt the bios to read a key.
		/// </summary>
		/// <returns>The Key.</returns>
		/// <param name="To">Variable.</param>
		public static byte[] ConsoleReadKey(byte To)
		{
			return new byte[]
			{
				Begin,Call,ReadKey,End,
				Begin,MovAddr,To,Split,arg0,End,
			};
		}

		/// <summary>
		/// Interrupt the bios to write.
		/// </summary>
		public static byte[] ConsoleWrite(string Message)
		{
			byte loc = ++next;
			List<byte> bytes = new List<byte> ();
			bytes = addBytes (bytes, new byte[]{
				Begin,Define,loc,Split,(byte)Encoding.ASCII.GetByteCount(Message),End,
				Begin,Assign,loc,Split
			});
			bytes = addBytes(bytes,Encoding.ASCII.GetBytes(Message));
			bytes = addBytes(bytes,new byte[]{
				End,
				Begin,MovAddr,arg0,Split,loc,End,
				Begin,Call,Write,End,
				Begin,Call,Show,End,
				Begin,Clear,arg0,End,
			});
			return bytes.ToArray();
		}

		/// <summary>
		/// Adds the bytes.
		/// </summary>
		/// <returns>The bytes.</returns>
		/// <param name="to">To.</param>
		/// <param name="from">From.</param>
		public static List<byte> addBytes(List<byte> to, byte[] from)
		{
			foreach (byte b in from) {
				to.Add (b);
			}
			return to;
		}
	}
}

