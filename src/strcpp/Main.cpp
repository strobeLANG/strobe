#include "Runtime.h"
#define Empty vector<Byte>(0)
using namespace str;
using namespace std;

void main(void)
{
	Runtime x = Runtime();
	Executable e;
	e.add(Instruction(Op_Allocate, { 0x1,0x0,0x0,0x0,0xfe,0x1,0x0,0x0,0x0 }));
	e.add(Instruction(Op_Assign, { 0x1,0xfe,0x1,0x0,0x0,0x0 }));
	e.add(Instruction(Op_Test, Empty));
	x.Run(e);
}