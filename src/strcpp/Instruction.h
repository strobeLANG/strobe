#pragma once
#include "Common.h"
namespace str
{
	// OpType
	enum OpType
	{
		Op_Null,
		Op_Add,
		Op_Subtract,
		Op_Divide,
		Op_Mutiply,
		Op_Allocate,
		Op_Assign,
		Op_Interrupt,
		Op_Compare,
		Op_Move,
		Op_Addr,
		Op_Goto,
		Op_Label,
		Op_Clear,
	};

	// Insturction Class
	class Instruction
	{
	public:
		OpType operation;
		vector<Byte> parameters;
	};
}