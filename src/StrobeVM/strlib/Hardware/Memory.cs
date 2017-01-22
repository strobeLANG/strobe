using System;
namespace StrobeVM.Hardware
{
	/// <summary>
	/// Memory.
	/// </summary>
	public class Memory
	{
		/// <summary>
		/// The ram.
		/// </summary>
		public byte[] Ram;
		/// <summary>
		/// The size.
		/// </summary>
		int Size;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:StrobeVM.Hardware.Memory"/> class.
		/// </summary>
		/// <param name="Size">Size.</param>
		public Memory(int Size)
		{
			this.Size = Size;
			Ram = new byte[Size];
			Clear();
		}
		/// <summary>
		/// Interrupt this instance.
		/// </summary>
		public byte[] Interrupt()
		{
			return BitConverter.GetBytes(Size);
		}
		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <returns>The size.</returns>
		public int GetSize()
		{
			return Size;
		}
		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < Size; i++)
				Ram[i] = 0;
		}
		/// <summary>
		/// Get the specified Address.
		/// </summary>
		/// <param name="Addr">Address.</param>
		public byte Get(int Addr)
		{
			return Ram[Addr];
		}
		/// <summary>
		/// Set the specified Address and Value.
		/// </summary>
		/// <param name="Addr">Address.</param>
		/// <param name="Value">Value.</param>
		public void Set(int Addr, byte Value)
		{
			Ram[Addr] = Value;
		}
	}
}
