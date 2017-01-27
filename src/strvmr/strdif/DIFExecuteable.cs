using System.Collections.Generic;
namespace StrobeVM.DIF
{
	/// <summary>
	/// DIF Executeable.
	/// </summary>
	public class DIFExecuteable : Executeable
	{
		/// <summary>
		/// Gets the instructions.
		/// </summary>
		/// <value>The instructions.</value>
		public List<DIFInstruction> insts { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:StrobeVM.DIF.DIFExecuteable"/> class.
		/// </summary>
		public DIFExecuteable() : base(Type.DIF)
		{
			insts = new List<DIFInstruction>();
		}

		/// <summary>
		/// Adds the instruction.
		/// </summary>
		/// <param name="ins">instruction.</param>
		public void AddInst(DIFInstruction ins)
		{
			insts.Add(ins);
		}

		/// <summary>
		/// Turn the instructions to executeable CPU instructions.
		/// </summary>
		public override Instruction[] CPU()
		{
			List<Instruction> inst = new List<Instruction>();
			foreach (DIFInstruction i in insts)
			{
				inst.Add(i.toInstruction());
			}
			return inst.ToArray();
		}
	}
}
