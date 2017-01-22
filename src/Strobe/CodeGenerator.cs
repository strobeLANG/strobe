using System.Text;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

/*
  
 	Found this creature on Quora,
	and it's called safety pig.

	It helps you with this terrible code.
	                         _
	 _._ _..._ .-',     _.._(`))
	'-. `     '  /-._.-'    ',/
	   )         \            '.
	  / _    _    |             \
	 |  a    a    /              |
	 \   .-.                     ;
	  '-('' ).-'       ,'       ;
	     '-;           |      .'
	        \           \    /
	        | 7  .__  _.-\   \
	        | |  |  ``/  /`  /
	       /,_|  |   /,_/   /
	          /,_/      '`-'

  I hope it works, because this code is messy.
  Edit: It worked, found the bug and fixed it!

 */

namespace Strobe
{
	/// <summary>
	/// Code generator.
	/// </summary>
	public class CodeGenerator
	{
		/// <summary>
		/// The lib folder.
		/// </summary>
		static string libf = Path.GetDirectoryName(
			Assembly.GetExecutingAssembly().Location) + "/lib/";
		
		/// <summary>
		/// The Result
		/// </summary>
		Result Res = new Result();

		/// <summary>
		/// The functions.
		/// </summary>
		Dictionary<string,Function> Functions = new Dictionary<string,Function>();

		/// <summary>
		/// The input.
		/// </summary>
		readonly ParseTree Input;

		/// <summary>
		/// The current variable.
		/// </summary>
		int CurrentVar;

		/// <summary>
		/// The variables.
		/// </summary>
		Dictionary<string,int> Vars = new Dictionary<string, int> ();

		/// <summary>
		/// The output.
		/// </summary>
		List<byte> Output = new List<byte>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Strobe.CodeGenerator"/> class.
		/// </summary>
		/// <param name="Input">Input.</param>
		public CodeGenerator (ParseTree Input)
		{
			// We use 0x2c because of safety.
			CurrentVar = 0x2c;
			this.Input = Input;
			// Generate the code.
			Generate();
		}

		/// <summary>
		/// Generate the code.
		/// </summary>
		void Generate()
		{
			// For include
			foreach (string s in Input.Preprocessor)
				Preprocess (s);

			// Actually import the namespaces & classes
			foreach (Namespace n in Input.Namespaces)
				foreach (Function f in n.Functions)
					Functions.Add (n.Name + f.Name, f);

			// Generate the code, starting with main
			// TODO: Pass arguments instead of null.
			Generate (findMain(),null,null);
		}

		/// <summary>
		/// Preprocess the specified string.
		/// </summary>
		/// <param name="s">String.</param>
		void Preprocess(string s)
		{
			// Hell..

			// Include
			if (s.StartsWith("include")) {
				// Remove the "include" from the string
				s = s.TrimStart("include".ToCharArray());
				// Remove the spaces.
				while(s.StartsWith(" "))
					s = s.TrimStart(" ".ToCharArray());
				// Include from /lib/
				if (s.StartsWith ("<")) {
					while (s.EndsWith (" "))
						s = s.TrimEnd (" ".ToCharArray ());
					// Remove < and >.
					s = s.TrimEnd (">".ToCharArray ());
					s = s.TrimStart ("<".ToCharArray ());

					// Add the lib folder.
					s = libf + s;

					// Compile the file.
					var x = new Parser (new Simplifier (new Lexer (File.ReadAllText (s))
							.get ().Tokens).get ().STokens).get ().Tree;

					// Add the namespaces
					foreach (Namespace y in x.Namespaces) {
						Input.Namespaces.Add (y);
					}

					// Peprocess
					foreach (string g in x.Preprocessor) {
						Preprocess (g);
					}
					// Load from current folder.
				} else if (s.StartsWith("\"")) {
					// Remove "'s
					s = s.TrimEnd ("\"".ToCharArray ());
					s = s.TrimStart ("\"".ToCharArray ());

					// Compile the file
					var x = new Parser (new Simplifier (new Lexer (File.ReadAllText (s))
						.get ().Tokens).get ().STokens).get ().Tree;

					// Add the namespaces
					foreach (Namespace y in x.Namespaces) {
						Input.Namespaces.Add (y);
					}

					// Preprocess
					foreach (string g in x.Preprocessor) {
						Preprocess (g);
					}
				}
			}
		}

		/// <summary>
		/// Adds the variable.
		/// </summary>
		/// <param name="Bytes">Bytes.</param>
		int AddVariable(byte[] Bytes)
		{
			// Get the current variable space and increase it by 1
			int current = ++CurrentVar;

			// Start allocating
			Output.Add(0x0);
			Output.Add(0x4);

			// Variable ID
			foreach (byte add in BitConverter.GetBytes(current))
				Output.Add(add);

			// Variable Size
			Output.Add(0xfe);
			foreach (byte add in BitConverter.GetBytes(Bytes.Length))
				Output.Add(add);

			// End allocating, and start assigning
			Output.Add(0xff);
			Output.Add(0x0);
			Output.Add(0x5);

			// Variable ID
			foreach(byte add in BitConverter.GetBytes(current))
				Output.Add(add);

			// Add the contents
			Output.Add(0xfe);
			foreach(byte add in Bytes)
				Output.Add(add);

			// End assigning and return the current variable
			Output.Add(0xff);
			return current;
		}

		/// <summary>
		/// Interrupt the specified byte.
		/// </summary>
		/// <param name="x">The byte.</param>
		void Interrupt(byte x)
		{
			//  Start Interrupt
			Output.Add (0x0);
			Output.Add (0x6);

			// Interrupt Byte
			Output.Add (x);

			// End Interrupt
			Output.Add (0xff);
		}

		/// <summary>
		/// Move the specified addresses.
		/// </summary>
		/// <param name="x">To.</param>
		/// <param name="y">From.</param>
		void Move(int x, int y)
		{
			// Start Move
			Output.Add(0x0);
			Output.Add(0x9);

			// Move to
			foreach (byte add in BitConverter.GetBytes(x))
				Output.Add (add);

			// Move from
			Output.Add(0xfe);
			foreach (byte add in BitConverter.GetBytes(y))
				Output.Add (add);

			// End Move
			Output.Add(0xff);
		}

		/// <summary>
		/// Generate from the specified function.
		/// </summary>
		/// <param name="func">Function.</param>
		void Generate(Function func, Args args, Function old)
		{
			/* 
			 * Welome to hell.
			 * Feel free to touch, but you rollback at the end.
			 */
			// Check for valid arguments
			if (func.Arguments.Arguments.Count != args?.Arguments.Count)
				// If it's not main, throw an exception
				if (args != null)
					throw new Exception ("Incorrect number of arguments");

			// Pass the arguments
			for (int i = 0; i < args?.Arguments.Count; i++) {
				// If the arguments are constant, define them.
				if (args.Arguments [i].isConst) {
					byte[] x;

					// Turn them into bytes, if string using ASCII, if number using BitConverter.
					if (args.Arguments [i].isNum) {
						x = BitConverter.GetBytes (int.Parse (args.Arguments [i].Name));
					} else {
						x = Encoding.ASCII.GetBytes (args.Arguments [i].Name);
					}

					// Register the variable.
					int var = AddVariable (x);

					// Check if to use direct (starting with x)
					if (func.Arguments.Arguments [i].Name.ToLower ().StartsWith ("x")) {
						// Move the contents to the address
						Move (int.Parse (func.Arguments.Arguments [i].Name.ToLower ().TrimStart ('x')), var);
					} else {
						// Check if it is already defined, if not, define it.
						if (!Vars.ContainsKey (func.Name + func.Arguments.Arguments [i].Name)) {
							// Define it.
							Vars.Add (func.Name + func.Arguments.Arguments [i].Name, var);
						} else {
							// Change the already defined variable.
							Vars [func.Name + func.Arguments.Arguments [i].Name] = var;
						}
					}
					// If the arguments are not constant, move them.
				} else {
					int var;

					// Check if it's direct, if it is, directly get the value
					if (args.Arguments [i].Name.ToLower ().StartsWith ("x"))
						var = int.Parse (args.Arguments [i].Name.ToLower ().TrimStart ('x'));
					else
						// If not, get the variable
						var = Vars[old.Name + args.Arguments [i].Name];

					// Check if should move to variable or address
					if (func.Arguments.Arguments [i].Name.ToLower ().StartsWith ("x")) {
						// Move the value directly
						Move (int.Parse (func.Arguments.Arguments [i].Name.ToLower ().TrimStart ('x')), var);
					} else {
						// Define new variable
						if (!Vars.ContainsKey (func.Name + func.Arguments.Arguments [i].Name)) {
							Vars.Add (func.Name + func.Arguments.Arguments [i].Name, var);
						} else {
							// Change the address
							Vars [func.Name + func.Arguments.Arguments [i].Name] = var;
						}
					}
				}
			}

			// Parse the instructions
			foreach (Instruction i in func.Instructions) {
				switch (i.Func.Function) {
					/*
					 * Define new variable from constant
					 */
				case "new":
						// Check if it has valid arguments
						if (i.Func.Arguments.Arguments.Count == 1 && i.Func.Arguments.Arguments [0].isConst) {
							byte[] x;
							// Check if it's a number or string
							if (i.Func.Arguments.Arguments [0].isNum) {
								// Number
								x = BitConverter.GetBytes(int.Parse (i.Func.Arguments.Arguments [0].Name));
							} else {
								// String
								x  = Encoding.ASCII.GetBytes(i.Func.Arguments.Arguments [0].Name);
							}
							// Define the variable
							int var = AddVariable(x);

							// Assign the value to the variable
							if (i.Op?.Type == "=") {
								// Check if it's valid
								if (i.Var?.isConst == false) {
									// Is it direct? (starts with x)
									if (i.Var.Name.ToLower ().StartsWith ("x"))
										// Directly move
										Move (int.Parse (i.Var.Name.ToLower ().TrimStart ('x')), var);
									else {
										// Define if not already defined
										if (!Vars.ContainsKey (func.Name + i.Var.Name)) {
											Vars.Add (func.Name + i.Var.Name, var);
										} else {
											// Change if already defined
											Vars [func.Name + i.Var.Name] = var;
										}
									}
								}
							}
							// Throw an exception
						} else throw new Exception ("Invalid arguments in `new`.");
					break;
						/*
						 * Get Value
						 */
				case "get":
						// Check for the arguments count
						if (i.Func.Arguments.Arguments.Count == 1) {
							int var;

							// If it starts with "x", directly load the value
							if (i.Func.Arguments.Arguments [0].Name.ToLower ().StartsWith ("x"))

								//Set the var to a parsed integer of the var name without "x"
								var = int.Parse (i.Var.Name.ToLower ().TrimStart ('x')); 
							else
								// Set the var to the pre-defined value of the var name.
								var = Vars[func.Name + i.Func.Arguments.Arguments [0].Name];

							// Check if sould assign
							if (i.Op?.Type == "=") {
								// Check if the variable is valid
								if (i.Var?.isConst == false) {

									// If it starts with "x", directly put into the variable
									if (i.Var.Name.ToLower ().StartsWith ("x"))
										// Move.
										Move (int.Parse (i.Var.Name.ToLower ().TrimStart ('x')), var);
									else {
										// If the variable isn't defined, define it.
										if (!Vars.ContainsKey (func.Name + i.Var.Name)) {
											Vars.Add (func.Name + i.Var.Name, var);
										} else {
											// Change the variable
											Vars [func.Name + i.Var.Name] = var;
										}
									}
								}
							}
						} else
							// Throw the exception
							throw new Exception ("Incorrect amount of arguments in `get`.");
					break;
						/*
						 * Interrupt
						 */
				case "int":
						// Check if the arguments are correct & interrupt
						if (i.Func.Arguments.Arguments.Count == 1 && i.Func.Arguments.Arguments[0].isNum)
						{
							int x = int.Parse(i.Func.Arguments.Arguments[0].Name);
							// Interrupt
							Interrupt((byte)x);
						} else
							// Thow the exception
							throw new Exception("Interrupt can only accept bytes as arguments.");
					break;
					/*
					 * Execute Function
					 */
					default:
						// Find the function
						Function f = find (i.Func.Namespace, i.Func.Function);
						// Set the function name to something unique to the namespace
						f.Name = i.Func.Namespace + " " + i.Func.Function + " ";
						// Generate the function, and return.
						Generate (f,i.Func.Arguments,func);
						// Return if it's a variable
						if (f.Ret?.Type == TokenType.Variable)
						{
							int var;
							// Check if should return address.
							if (f.Ret.Value.ToLower().StartsWith("x"))
								// Get direct address.
								var = int.Parse(f.Ret.Value.ToLower().TrimStart('x'));
							else
								// Get variable.
								var = Vars[f.Name + f.Ret.Value];
							// Check if should assign
							if (i.Op?.Type == "=")
							{
								if (i.Var?.isConst == false)
								{
									// Check if it's a direct address
									if (i.Var.Name.ToLower().StartsWith("x"))
										// Move the address
										Move(int.Parse(i.Var.Name.ToLower().TrimStart('x')), var);
									else {
										// Define the variable if it's not already defined
										if (!Vars.ContainsKey(func.Name + i.Var.Name))
										{
											// Define Variable
											Vars.Add(func.Name + i.Var.Name, var);
										}
										else {
											// Change Variable
											Vars[func.Name + i.Var.Name] = var;
										}
									}
								}
							}
						}
						else {
							// Make sure that it's an invalid return and not a void
							if (f.Ret?.Type == TokenType.Number
								|| f.Ret?.Type == TokenType.Register
								|| f.Ret.Type == TokenType.String)
								throw new Exception("Functions can only return variables!");
						}
						// Collect the garbadge
						CollectGarbadge(f.Name);
						break;
				}
			}
		}

		/// <summary>
		/// Clears the variable.
		/// </summary>
		/// <param name="Var">Variable.</param>
		void ClearVar(int Var)
		{
			// Start Clear
			Output.Add(0x0);
			Output.Add(0xc);

			// Variable (as Bytes)
			foreach (byte add in BitConverter.GetBytes(Var))
				Output.Add(add);

			// End Clear
			Output.Add(0xff);
		}

		/// <summary>
		/// Frees the variable.
		/// </summary>
		/// <param name="Var">Variable.</param>
		void FreeVar(int Var)
		{
			/*
			 * Not implemented in kernel yet, but the idea is:
			 * - Remove all traces of the variable;
			 * - Free the space;
			 * - Pull all the variables down to the empty space;
			 * - Move the free address number down to the last variable;
			 */
		}

		/// <summary>
		/// Collects the garbadge.
		/// </summary>
		/// <param name="FuncName">Function name.</param>
		void CollectGarbadge(string FuncName)
		{
			// To avoid a bug.
			var Varc = new Dictionary<string, int>(Vars);
			foreach (KeyValuePair<string,int> v in Varc)
			{
				if (v.Key.StartsWith(FuncName))
				{
					ClearVar(v.Value);
					FreeVar(v.Value);
					Vars.Remove(v.Key);
				}
			}
		}

		/// <summary>
		/// Find the specified function.
		/// </summary>
		/// <param name="namesp">Namespace.</param>
		/// <param name="name">Function.</param>
		Function find(string namesp, string name)
		{
			// Check if the function is loaded
			if (Functions.ContainsKey (namesp + name))
				return Functions [namesp + name];

			// Nope, throw an exception
			throw new Exception ("Unable to find function `"+namesp+"."+name+"`");
		}

		/// <summary>
		/// Finds the main function.
		/// </summary>
		/// <returns>The main.</returns>
		Function findMain()
		{
			// Search for main
			foreach (Namespace name in Input.Namespaces)
				foreach (Function func in name.Functions)
					if (func.Name.ToLower () == "main")
						return func;

			// No main, throw an exception
			throw new Exception("No `Main` function was found!");
		}

		/// <summary>
		/// Get the result.
		/// </summary>
		public CodeGeneratorResult get()
		{
			return new CodeGeneratorResult
			{
				Errors = Res.Errors,
				Warnings = Res.Warnings,
				Code = Output.ToArray(),
			};
		}
	}
}