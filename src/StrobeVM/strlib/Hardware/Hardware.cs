using StrobeVM.Firmware;
namespace StrobeVM.Hardware
{
	/// <summary>
	/// Hardware.
	/// </summary>
	public class Hardware
	{
		/// <summary>
		/// The kernel.
		/// </summary>
		Kernel kernel;

		/// <summary>
		/// The bios.
		/// </summary>
		BIOS bios;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:StrobeVM.Hardware.Hardware"/> class.
		/// </summary>
		/// <param name="hInfo">Hardware info.</param>
		/// <param name="kernel">Kernel.</param>
		public Hardware(HardwareInfo hInfo, Kernel kernel)
		{
			this.kernel = kernel;
			bios = new BIOS();
		}

		/// <summary>
		/// Display error message.
		/// </summary>
		/// <param name="from">Source.</param>
		/// <param name="i">Code.</param>
		public void Error(string from, int i)
		{
			bios.Write(from + " Error (" + i + ")\n");
			bios.Display();
			throw new System.Exception(from + " Error (" + i + ")");
		}

		/// <summary>
		/// Interrupt using the specified parameter.
		/// </summary>
		/// <param name="Param">Parameter.</param>
		public byte[] Interrupt(byte Param)
		{
			switch (Param)
			{
				case 0:
					// Too brutal.
					//bios.Exit(kernel.AMem(1)[0]);
					kernel.Stop();
					return new byte []{ };
				case 1:
					bios.Write(kernel.AMem(1));
					return new byte[]{ };
				case 2:
					bios.Display();
					return new byte[] { };
				case 3:
					return bios.Read();
				case 4:
					return bios.ReadLine();
				case 5:
					return bios.ReadKey();
				case 6:
					return bios.ReadFile(kernel.AMem(1));
				case 7:
					bios.WriteFile(kernel.AMem(1),kernel.AMem(2));
					return new byte[] { };
				case 8:
					bios.AppendFile(kernel.AMem(1), kernel.AMem(2));
					return new byte[] { };
				case 9:
					bios.CreateFolder(kernel.AMem(1));
					return new byte[] { };
				case 10:
					return bios.FileExists(kernel.AMem(1));
				case 11:
					return bios.FolderExists(kernel.AMem(1));
				case 12:
					bios.DeleteFile(kernel.AMem(1));
					return new byte[] { };
				case 13:
					bios.DeleteFolder(kernel.AMem(1));
					return new byte[] { };
				default:
					Error("CPU", 6);
					bios.Exit(1);
					return new byte[]{ };
			}
		}
		/// <summary>
		/// Hardware info.
		/// </summary>
		public struct HardwareInfo
		{
			// Nothing for now...
		}
	}
}
