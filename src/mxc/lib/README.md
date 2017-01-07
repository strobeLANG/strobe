# The Libraries

* Put 32-bit libraries in /32/
* Put 64 bit libraries in /64/

Put universal libraries in here.

## Good Library Naming Tutorial

* All characters are lower case.
* All linux libraries start with `u`.
* All windows libraries start with `win`.
* The univeral libraries don't have a prefix.
* Make sure the file ends wiht `.mxs`.
* Make it short and understandable.

### Some Examples

* library `MakeStuff` for Win32, name it:
`winmakest.mxs`, save to: `lib/32`;
* library `ComplexMath` for everyone, name it:
`comath.mxs`, save to `lib`;
* library `Math64` for 64-bit Unix, name it:
`umath64.mxs`, save it to `lib/64`;
* library `Something` for Unix, name it:
`usomet.mxs`, save it to `lib`;

### Did you actually made a working library?

* Contribute if you want to help us out!
