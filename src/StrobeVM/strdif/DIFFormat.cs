using System;
namespace StrobeVM.DIF
{
	/*
	 * Direct Instruction Format
	 * 
 	 * Every instruction starts with 0x0;
	 * Every instruction ends with 0x255;
	 * Before the contents you must specify the OpType;
	 * Ex: 0x0 0x6 0x1 0x2 0x3 0x255
	 * OpType: 0x6
	 * Contents: 0x1 0x2 0x3
	 */

	/// <summary>
	/// DIF Format.
	/// </summary>
	public class DIFFormat : Format
	{
		/// <summary>
		/// Load the specified Input.
		/// </summary>
		/// <param name="Input">Input.</param>
		public override Executeable Load(byte[] Input)
		{
			DIFExecuteable Return = new DIFExecuteable();
			DIFInstruction cInst = new DIFInstruction(Instruction.OpType.Null);
			byte cNow; int cPos =0;
			while (cPos < Input.Length)
			{
				cNow = Input[cPos];
				switch (cNow)
				{
					case 0:
						if (cInst.Op == Instruction.OpType.Null)
						{
							cInst = new DIFInstruction(OpTypeFromByte(Input[++cPos]));
						}
						else {
							cInst.Param.Add(cNow);
						}
						break;
					case 255:
						Return.AddInst(cInst);
						cInst = new DIFInstruction(Instruction.OpType.Null);
						break;
					default:
						cInst.Param.Add(cNow);
						break;
				}
				cPos++;
			}
			return Return;
		}
		/// <summary>
		/// Turn the Operator Byte to Operator Type.
		/// </summary>
		/// <returns>The type from byte.</returns>
		/// <param name="OpByte">Op byte.</param>
		public Instruction.OpType OpTypeFromByte(byte OpByte)
		{
			switch (OpByte)
			{
				case 0:
					return Instruction.OpType.Add;
				case 1:
					return Instruction.OpType.Subtract;
				case 2:
					return Instruction.OpType.Divide;
				case 3:
					return Instruction.OpType.Mutiply;
				case 4:
					return Instruction.OpType.Allocate;
				case 5:
					return Instruction.OpType.Assign;
				case 6:
					return Instruction.OpType.Interrupt;
				case 7:
					return Instruction.OpType.Compare;
				case 8:
					return Instruction.OpType.Move;
				case 9:
					return Instruction.OpType.Addr;
				case 10:
					return Instruction.OpType.Label;
				case 11:
					return Instruction.OpType.Goto;
				case 12:
					return Instruction.OpType.Clear;
				default:
					throw new Exception("Incorrect OpType: " + (int)OpByte);
			}
		}
	}
}
