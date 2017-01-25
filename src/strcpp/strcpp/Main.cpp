#include "BIOS.h"
#pragma once
using namespace str::firmware;
void main()
{
	BIOS x = BIOS();
	x.Write("Hello World\n");
	x.Display();
	system("Pause");
}