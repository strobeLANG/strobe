#include "DIFFormat.h"
#include "Runtime.h"
using namespace str::runtime;
void main(int argc, char *argv[])
{
	// Define the format (DIF)
	DIFFormat format;

	// Define file to open
	char *file;

	// Define the size (1MB)
	int size = 1024 * 1024;

	// Define the runtime
	Runtime runtime = Runtime(size);

	// Check if there are arguments
	if (argc == 0)
	{
		runtime.bios.Write("The syntax of the command is incorrect.");
		runtime.bios.Display();
		return;
	}

	// Open the files
	for (int i = 0; i < argc; i++)
	{
		runtime.Start(format.Load(str::ReadAllBytes(argv[i])));
	}

	// Step in the runtime
	while (runtime.Running())
	{
		runtime.Step();
	}
}