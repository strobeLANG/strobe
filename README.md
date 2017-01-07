# The Strobe Compiler
The Strobe Compiler compiles Strobe code into assembler code for the `NASM` assembler.
It isn't currently working because the parser needs to be re-worked and the code generator needs to be started.

Other than this, it doesn't have any known bugs, if you find some report them at [Issues](//github.com/mihail-mojsoski/Strobe/issues), so we can fix them.

If you want to see how will the Strobe syntax look like, here it is:

```
#include "usys.str"

namespace Name {
	function Main() {
		Console.WriteLine("Hello World");
		System.Exit();
	}
}
```

The syntax is influenced by the following programming languages:

 - C - for the pre-processor instructions and comments;
 - X# - for the namespaces, functions and idea;
 - C# - for the namespaces and library names;
 - PHP - for the variables and functions;

###Do you want to support us?
#### Contribute!
#### Make a library!
#### Find Bugs!
#### Give us ideas!
