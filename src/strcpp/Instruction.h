#pragma once
#include "Common.h"
using namespace std;
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
		
		// Initialize
		Instruction(OpType optype, vector<Byte> byte)
		{
			operation = optype;
			parameters = byte;
		}
	};

	// Executable class
	class Executable
	{
	public:
		vector<Instruction> insturctions;

		// Add an instruction
		void add(Instruction i)
		{
			insturctions.push_back(i);
		}
	};
}