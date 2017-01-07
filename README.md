# The MX Compiler
The MX Compiler compiles MX code into assembler code for the `NASM` assembler. It isn't currently working because the code generator isn't implemented yet and the parser has a few bugs that are needed to be fixed and they are:

 - `Parser.Instruction()`: algorithm is not optimized;
 - `Parser.Instruction()`: not setting the variables inside `Instruction_current.Op`, `Instruction_current.Var` and `Instruction_current.Type` correctly;
 - `ParseTree`: some types are not optimized;
 - `CodeGenerator`: is not started working on;

Other than this, it doesn't have any known bugs, if you find some report them at [Issues](https://github.com/mihail-rotmg/mx/issues), so we can fix them.

If you want to see how will the MX syntax look like, here it is:

```
#include<usys.mxs>
#include<string.mxs>
/*
	This is a comment.
*/
namespace MyConsoleApplication
{
	function Main()
	{
		$var = String.new("Hello World");
		Console.Write($var);
		System.Exit();
		return $var; // This is just an example.
	}
}
```
You use `#include<filename>` and `#include "filename"` to include files, exactly like C and C++, as you can see functions are put in namespaces, then you can see that there are no data types for functions (it is specified by the returned type), to define a string constant you need to use the `string.mxs`'s `String.new()`, it adds the string to the `.data` section, and functions are called like this `namespace.function($arg1, $arg2)`, at the end of the program you must type `System.Exit();` or you won't exit clean, or if you made an other function type `return $someting;` to return `$something`  back to the caller.

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
