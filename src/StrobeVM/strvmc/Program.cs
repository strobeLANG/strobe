using StrobeVM.DIF;
using StrobeVM;
using StrobeVM.Firmware;
using System.IO;

namespace strvmc
{
	/// <summary>
	/// Strobe VMC.
	/// </summary>
	class StrobeVMC
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="param">The command-line arguments.</param>
		public static void Main(string[] param)
		{
			// The kernel has 1024 bytes of memory, change this if you want to.
			Kernel kernel = new Kernel(1024);
			int i = 0;

			// Uncomment this if you want to load files
			//foreach (string s in param)
			{
				try
				{
					// Load the executeable using the DIF Format
					//Executeable x = new DIFFormat().Load(File.ReadAllBytes(s));

					// Uncomment the previous line if you want to load bytes from file.
					// Load the "Hello World" test.
					Executeable x = new DIFFormat().Load(Tests.KeyRead());

					// Start the application in the kernel
					kernel.Start(x);
				}
				catch
				{
					// Error while loading, increase the counter
					i++;
				}
			}

			// Step the kernel while it has running processes...
			while (kernel.running.Count > 0)
			{
				kernel.Step();
			}
		}
	}
}
