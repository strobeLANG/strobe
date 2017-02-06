using System.Collections.Generic;
using System;
using StrobeVM.Firmware;

namespace StrobeVM.Hardware
{
    /// <summary>
    /// Processor.
    /// </summary>
    public class Processor
	{
		/// <summary>
		/// The hardware.
		/// </summary>
		Hardware hardware;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:StrobeVM.Hardware.Processor"/> class.
		/// </summary>
		/// <param name="hInfo">Hardware info.</param>
		/// <param name="kernel">Kernel.</param>
		public Processor(Hardware.HardwareInfo hInfo, Kernel kernel)
		{
			hardware = new Hardware(hInfo, kernel);
		}
		/// <summary>
		/// Execute the specified instruction.
		/// </summary>
		/// <param name="i">The instruction.</param>
		public byte[] Execute(Instruction i)
		{
			switch (i.Op)
			{
				case Instruction.OpType.Clear:
				case Instruction.OpType.Label:
				case Instruction.OpType.Goto:
				case Instruction.OpType.Move:
				case Instruction.OpType.Assign:
				case Instruction.OpType.Addr:
				case Instruction.OpType.Allocate:
					hardware.Error("Kernel", 3);
					return null;
				case Instruction.OpType.Compare:
					return Comp(i.Param);
				case Instruction.OpType.Interrupt:
					return Inte(i.Param);
				case Instruction.OpType.Subtract:
					return Sub(i.Param);
				case Instruction.OpType.Mutiply:
					return Mul(i.Param);
				case Instruction.OpType.Add:
					return Add(i.Param);
				case Instruction.OpType.Divide:
					return Div(i.Param);
				default:
					hardware.Error("CPU",0);
					return null;
			}
		}
		/// <summary>
		/// Interupts with the pecified parameter.
		/// </summary>
		/// <param name="Param">Parameter.</param>
		byte[] Inte(byte[] Param)
		{
			if (Param.Length != 1)
			{
				hardware.Error("CPU",1);
				return null;
			}
			return hardware.Interrupt(Param[0]);
		}

		/// <summary>
		/// Compare the specified arguments.
		/// </summary>
		/// <param name="ar">Arguments.</param>
		byte[] Comp(byte[] ar)
		{
			if (ar.Length < 3)
			{
				hardware.Error("CPU", 4);
				return null;
			}
			switch (ar[0])
			{
				case 0:
					return Equ(RemoveFirst(ar));
				case 1:
					return Neq(RemoveFirst(ar));
				case 2:
					return Lss(RemoveFirst(ar));
				case 3:
					return Mor(RemoveFirst(ar));
				default:
					hardware.Error("CPU", 5);
					return null;
			}
		}

        /// <summary>
        /// Remove the first element from a byte array.
        /// </summary>
        /// <returns></returns>
        byte[] RemoveFirst(byte[] arr)
        {
            List<byte> n = new List<byte>();
            for (int i = 0; i < arr.Length; i++)
                n.Add(arr[i]);
            n.RemoveAt(0);
            return n.ToArray();
        }

		/// <summary>
		/// Display error message, and halt.
		/// </summary>
		/// <param name="i">The error ID.</param>
		public void Error(int i)
		{
			hardware.Error("Kernel", i);
		}

		/// <summary>
		/// Comparation: Equal
		/// </summary>
		/// <param name="ar">Arugments.</param>
		byte[] Equ(byte[] ar)
		{
			int[] ret = TwoArgs(ar);
            ret[0] = BitConverter.ToInt32(hardware.kernel.AMem(ret[0]), 0);
            ret[1] = BitConverter.ToInt32(hardware.kernel.AMem(ret[1]), 0);
            return BitConverter.GetBytes(ret[0] == ret[1]);
		}

		/// <summary>
		/// Halt this instance.
		/// </summary>
		public void Halt()
		{
			throw new Exception ("CPU Halt");
		}

		/// <summary>
		/// Comparation: Not Equal
		/// </summary>
		/// <param name="ar">Arugments.</param>
		byte[] Neq(byte[] ar)
		{
			int[] ret = TwoArgs(ar);
            ret[0] = BitConverter.ToInt32(hardware.kernel.AMem(ret[0]), 0);
            ret[1] = BitConverter.ToInt32(hardware.kernel.AMem(ret[1]), 0);
            return BitConverter.GetBytes(ret[0] != ret[1]);
		}

		/// <summary>
		/// Comparation: More
		/// </summary>
		/// <param name="ar">Arguments.</param>
		byte[] Mor(byte[] ar)
		{
			int[] ret = TwoArgs(ar);
            ret[0] = BitConverter.ToInt32(hardware.kernel.AMem(ret[0]), 0);
            ret[1] = BitConverter.ToInt32(hardware.kernel.AMem(ret[1]), 0);
            return BitConverter.GetBytes(ret[0] > ret[1]);
		}

		/// <summary>
		/// Comparation: Less
		/// </summary>
		/// <param name="ar">Arguments.</param>
		byte[] Lss(byte[] ar)
		{
			int[] ret = TwoArgs(ar);
            ret[0] = BitConverter.ToInt32(hardware.kernel.AMem(ret[0]), 0);
            ret[1] = BitConverter.ToInt32(hardware.kernel.AMem(ret[1]), 0);
            return BitConverter.GetBytes(ret[0] < ret[1]);
		}

		/// <summary>
		/// Operation: Subtraction
		/// </summary>
		/// <param name="ar">Arguments.</param>
		byte[] Sub(byte[] ar)
		{
			int[] ret = TwoArgs(ar);
            ret[0] = BitConverter.ToInt32(hardware.kernel.AMem(ret[0]), 0);
            ret[1] = BitConverter.ToInt32(hardware.kernel.AMem(ret[1]), 0);
            return BitConverter.GetBytes(ret[0] - ret[1]);
		}

		/// <summary>
		/// Operation: Multiplication
		/// </summary>
		/// <param name="ar">Arguments.</param>
		byte[] Mul(byte[] ar)
		{
			int[] ret = TwoArgs(ar);
            ret[0] = BitConverter.ToInt32(hardware.kernel.AMem(ret[0]), 0);
            ret[1] = BitConverter.ToInt32(hardware.kernel.AMem(ret[1]), 0);
            return BitConverter.GetBytes(ret[0] * ret[1]);
		}

		/// <summary>
		/// Operation: Division
		/// </summary>
		/// <param name="ar">Arguments.</param>
		byte[]  Div(byte[] ar)
		{
			int[] ret = TwoArgs(ar);
            ret[0] = BitConverter.ToInt32(hardware.kernel.AMem(ret[0]), 0);
            ret[1] = BitConverter.ToInt32(hardware.kernel.AMem(ret[1]), 0);
            return BitConverter.GetBytes(ret[0] / ret[1]);
		}

		/// <summary>
		/// Operation: Additon
		/// </summary>
		/// <param name="ar">Arguments.</param>
		byte[] Add(byte[] ar)
		{
			int[] ret = TwoArgs(ar);
            ret[0] = BitConverter.ToInt32(hardware.kernel.AMem(ret[0]), 0);
            ret[1] = BitConverter.ToInt32(hardware.kernel.AMem(ret[1]), 0);
            return BitConverter.GetBytes(ret[0] + ret[1]);
		}

		/// <summary>
		/// Gets two integer arguments from byte array.
		/// </summary>
		/// <returns>The arguments.</returns>
		/// <param name="ar">4 bytes</param>
		public int[] TwoArgs(byte[] ar)
		{
			bool isTwo = false;
			List<byte> one = new List<byte>();
			List<byte> two = new List<byte>();
			foreach (byte b in ar)
			{
				if (b == 254)
				{
					isTwo = true;
					continue;
				}
				if (isTwo == false)
				{
					one.Add(b);
				}
				else
				{
					two.Add(b);
				}
			}
			while (one.Count < 4)
			{
				one.Add(0);
			}
			while (two.Count < 4)
			{
				two.Add(0);
			}
			while (one.Count > 4)
			{
				one.RemoveAt(one.Count - 1);
			}
			while (two.Count > 4)
			{
				two.RemoveAt(two.Count - 1);
			}
			int x = BitConverter.ToInt32 (one.ToArray (), 0);
			int y = BitConverter.ToInt32 (two.ToArray (), 0);
			return new int[] {
				x,y
			};
		}
	}
}