using System.Collections.Generic;
using StrobeVM.Hardware;
namespace StrobeVM.Firmware
{
	/// <summary>
	/// Kernel.
	/// </summary>
	public class Kernel
	{
		/// <summary>
		/// Gets the free address.
		/// </summary>
		/// <value>The free address.</value>
		public int FreeAddr { private set; get;	}

		/// <summary>
		/// Gets or sets the labels.
		/// </summary>
		/// <value>The labels.</value>
		public Dictionary<int, int> Labels { set; get;}

		/// <summary>
		/// Gets the processor.
		/// </summary>
		/// <value>The processor.</value>
		public Processor proc { private set; get; }

		/// <summary>
		/// Gets the current process.
		/// </summary>
		/// <value>The current process.</value>
		public int currentprocess { private set; get; }

		/// <summary>
		/// Gets the memory.
		/// </summary>
		/// <value>The memory.</value>
		public Memory mem { private set; get; }

		/// <summary>
		/// Gets the running processes.
		/// </summary>
		/// <value>The processes.</value>
		public List<Process> running { private set; get; }

		/// <summary>
		/// Gets the location.
		/// </summary>
		/// <value>The location.</value>
		public List<int> loc  { private set; get; }

		/// <summary>
		/// Gets the allocated variables.
		/// </summary>
		/// <value>The alloc.</value>
		public Dictionary<int, Variable> Alloc { private set; get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:StrobeVM.Firmware.Kernel"/> class.
		/// </summary>
		/// <param name="RamSize">Ram size.</param>
		public Kernel(int RamSize)
		{
			Labels = new Dictionary<int, int>();
			loc = new List<int>();
			running = new List<Process>();
			mem = new Memory(RamSize);
			Alloc = new Dictionary<int, Variable>();
			proc = new Processor(new Hardware.Hardware.HardwareInfo(), this);
			FreeAddr = 0;
			currentprocess = 0;
			InitRegs(16);
		}

		/// <summary>
		/// Initializes the registers.
		/// </summary>
		/// <param name="n">N.</param>
		void InitRegs(int n)
		{
			for (int i = 0; i < n; i++)
			{
				AMem(i,FreeAddr,4);
			}
		}

		public Executeable[] Save()
		{
			List<Executeable> exec = new List<Executeable> ();
			foreach (Process p in running) {
				exec.Add (p.Exec);
			}
			return exec.ToArray ();
		}

		/// <summary>
		/// Start the specified executable.
		/// </summary>
		/// <param name="e">Executeable.</param>
		public void Start(Executeable e)
		{
			loc.Add(0);
			running.Add(new Process(e));
		}

		/// <summary>
		/// Execute the specified instruction.
		/// </summary>
		/// <param name="now">Instruction.</param>
		void Execute(Instruction now)
		{
			switch (now.Op)
			{
				case Instruction.OpType.Assign:
					int[] d = proc.TwoArgs(now.Param);
					var by = new List<byte>();
					bool use = false;
					foreach (byte z in now.Param)
					{
						if (use)
						{
							by.Add(z);
						}
						use |= z == 254;
					}
					AMem(d[0], by.ToArray());
					break;
				case Instruction.OpType.Addr:
					int[] ab = proc.TwoArgs(now.Param);
					AMem(ab[0], ab[1]);
					break;
				case Instruction.OpType.Move:
					int[] a = proc.TwoArgs(now.Param);
					AMem(a[0], AMem(a[1]));
					break;
				case Instruction.OpType.Allocate:
					int[] p = proc.TwoArgs(now.Param);
					AMem(p[0], FreeAddr, p[1]);
					break;
				case Instruction.OpType.Label:
					int[] h = proc.TwoArgs(now.Param);
					Labels.Add(h[0],loc[currentprocess - 1]);
					break;
				case Instruction.OpType.Goto:
					int[] c = proc.TwoArgs(now.Param);
					if (Labels.ContainsKey(c[0]))
					if (AMem(c[1])[0] == 0x0)
						loc[currentprocess - 1] = Labels[c[0]];
					break;
				case Instruction.OpType.Clear:
					int[] zvzx = proc.TwoArgs(now.Param);
					AMemClear(zvzx[0]);
					break;
				default:
					AMem(0, proc.Execute(now));
					break;
			}

		}

		/// <summary>
		/// Step this instance.
		/// </summary>
		public void Step()
		{
			if (currentprocess < running.Count)
			{
				Instruction now = running[currentprocess].Step(loc[currentprocess]);
				loc[currentprocess]++;
				currentprocess++;
				Execute(now);
			}
				else
			{
				currentprocess = 0;
				return;
			}

		}

		/// <summary>
		/// Move the address and size.
		/// </summary>
		/// <param name="dest">Destination.</param>
		/// <param name="source">Source.</param>
		public void AMem(int dest, int source)
		{
			if (Alloc.ContainsKey(dest))
			{
				if (Alloc.ContainsKey(source))
				{
					Alloc[dest] = new Variable(Alloc[source].Address,Alloc[source].Size);
					return;
				}
				proc.Error(11);
			}
			proc.Error(10);
		}

		/// <summary>
		/// Gets the range from memory.
		/// </summary>
		/// <returns>The range.</returns>
		/// <param name="Start">Start.</param>
		/// <param name="End">End.</param>
		public byte[] GetRange(int Start, int End)
		{
			int now = Start;
			List<byte> get = new List<byte>();
			while (now < Start + End)
				get.Add(mem.Get(now++));
			return get.ToArray();
		}

		/// <summary>
		/// Changes value of an allocated variable.
		/// </summary>
		/// <param name="set">ID.</param>
		/// <param name="value">Value.</param>
		public void AMem(int set, byte[] value)
		{
			if (Alloc.ContainsKey(set))
			{
				try
				{
					for (int i = 0; i < value.Length; i++)
						mem.Set(i + Alloc[set].Address, value[i]);
				}
				catch {
					proc.Error(8);
				}
			}
			else {
				proc.Error(7);
			}
		}
		/// <summary>
		/// Allocates memory.
		/// </summary>
		/// <param name="set">Id.</param>
		/// <param name="addr">Address.</param>
		/// <param name="size">Size.</param>
		public void AMem(int set, int addr, int size)
		{
			if (Alloc.ContainsKey(set))
			{
				return;
			}
			FreeAddr += size;
			Alloc.Add(set,new Variable(addr,size));
		}

		/// <summary>
		/// Gets id from allocated memory.
		/// </summary>
		/// <returns>The Memory.</returns>
		/// <param name="get">Id.</param>
		public byte[] AMem(int get)
		{
			if (Alloc.ContainsKey(get))
			{
				return GetRange(Alloc[get].Address, Alloc[get].Size);
			}
			return new byte[] { 0 };
		}


		/// <summary>
		/// Clear the Memory.
		/// </summary>
		/// <param name="clear">Id.</param>
		public void AMemClear(int clear)
		{
			if (Alloc.ContainsKey(clear))
			{
				int now = Alloc[clear].Address;
				while (now < Alloc[clear].Address + Alloc[clear].Size)
					mem.Set(now++, 0x0);
			}
			return;

		}

		/// <summary>
		/// Variable.
		/// </summary>
		public struct Variable
		{
			public int Address;
			public int Size;
			/// <summary>
			/// Initializes a new instance of the <see cref="T:StrobeVM.Firmware.Kernel.Variable"/> struct.
			/// </summary>
			/// <param name="addr">Address.</param>
			/// <param name="size">Size.</param>
			public Variable(int addr, int size)
			{
				Address = addr;
				Size = size;
			}
		}

		/// <summary>
		/// Process.
		/// </summary>
		public struct Process
		{
			private Instruction[] Inst;
			public Executeable Exec { get; private set;}
			public bool isRunning;
			/// <summary>
			/// Initializes a new instance of the <see cref="T:StrobeVM.Firmware.Kernel.Process"/> struct.
			/// </summary>
			/// <param name="e">E.</param>
			public Process(Executeable e)
			{
				isRunning = true;
				Inst = e.CPU();
				Exec = e;
			}
			/// <summary>
			/// Step the specified step.
			/// </summary>
			/// <param name="now">Current step.</param>
			public Instruction Step(int now)
			{
				if (now < Inst.Length)
					return Inst[now];

				isRunning = false;
				return null;
			}
		}
	}
}
