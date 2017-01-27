#include "DIFFormat.h"
namespace str
{
	namespace dif
	{
		// ByteFromOpType Definition
		byte DIFFormat::ByteFromOpType(OpType OpType)
		{
			switch (OpType)
			{
			case Add:
				return 0;
			case Subtract:
				return 1;
			case Divide:
				return 2;
			case Mutiply:
				return 3;
			case Allocate:
				return 4;
			case Assign:
				return 5;
			case Interrupt:
				return 6;
			case Compare:
				return 7;
			case Move:
				return 8;
			case Addr:
				return 9;
			case Label:
				return 10;
			case Goto:
				return 11;
			case Clear:
				return 12;
			default:
				throw("Incorrect OpType: " + OpType);
			}
		}

		// OpTypeFromByte Definition
		OpType DIFFormat::OpTypeFromByte(byte OpByte)
		{
			switch (OpByte)
			{
			case 0:
				return Add;
			case 1:
				return Subtract;
			case 2:
				return Divide;
			case 3:
				return Mutiply;
			case 4:
				return Allocate;
			case 5:
				return Assign;
			case 6:
				return Interrupt;
			case 7:
				return Compare;
			case 8:
				return Move;
			case 9:
				return Addr;
			case 10:
				return Label;
			case 11:
				return Goto;
			case 12:
				return Clear;
			default:
				throw("Incorrect OpByte: " + (int)OpByte);
			}
		}

	}
}