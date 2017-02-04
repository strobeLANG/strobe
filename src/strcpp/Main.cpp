#include "Runtime.h"
#define NULLVECTOR vector<Byte>(0)
using namespace str;
using namespace std;

void main(void)
{
	Runtime x = Runtime();
	Executable e;
	e.add(Instruction(Op_Subtract, NULLVECTOR));
	x.Run(e);
}