using System.Collections.Generic;
namespace StrobeVM.DIF
{
	/// <summary>
	/// DIF Instruction.
	/// </summary>
	public class DIFInstruction
	{
		/// <summary>
		/// Gets the operator type.
		/// </summary>
		/// <value>The operator type.</value>
		public Instruction.OpType Op {get; private set;}

		/// <summary>
		/// The parameter.
		/// </summary>
		public List<byte> Param = new List<byte>();

		/// <summary>
		/// Turn this instruction to CPU instruction.
		/// </summary>
		/// <returns>The instruction.</returns>
		public Instruction toInstruction()
		{
			return new Instruction(Op, Param.ToArray());
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:StrobeVM.DIF.DIFInstruction"/> class.
		/// </summary>
		/// <param name="op">Operator Type.</param>
		public DIFInstruction(Instruction.OpType op)
		{
			Op = op;
		}
	}
}
