# The Strobe Compiler
The Strobe Compiler compiles Strobe code into assembler code for the `NASM` assembler.
It isn't currently working because the parser needs to be re-worked and the code generator needs to be started.

Other than this, it doesn't have any known bugs, if you find some report them at [Issues](//github.com/mihail-mojsoski/Strobe/issues), so we can fix them.

If you want to see how will the Strobe syntax look like, here it is:

```
#include<usys.str>
#include<string.str>
/*
	This is a comment.
*/
namespace MyConsoleApplication
{
	function Main()
	{
		$var = new("Hello World");
		Console.Write($var);
		System.Exit();
		return $var; // This is just an example.
	}
}
```
You use `#include<filename>` and `#include "filename"` to include files, exactly like C and C++, as you can see functions are put in namespaces, then you can see that there are no data types for functions (it is specified by the returned type), to define a string constant you need to use the `new()` method, it adds the string to the `.data` section, and functions are called like this `namespace.function($arg1, $arg2)`, at the end of the program you must type `System.Exit();` or you won't exit clean, or if you made an other function type `return $someting;` to return `$something`  back to the caller.

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
