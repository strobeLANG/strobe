using System.IO;
using System;
namespace MX.Test
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Compiling...");
			Console.ResetColor();
			if (args.Length > 0)
			{
				string name = args[0];
				if (!File.Exists(name))
				{
					Console.WriteLine("Error 0: \"File does not exist\" at 0");
				}
				else {
					string input = File.ReadAllText(name);
					CompilerResult result = new Compiler(input).compile();
					foreach (Error e in result.Errors)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Error {0}: \"{1}\" at {2}", e.Code, e.Value, WriteLoc(name, e.Location));
						Console.ResetColor();
					}
					foreach (Warning e in result.Warnings)
					{
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.WriteLine("Warning {0}: \"{1}\" at {2}", e.Code, e.Value, WriteLoc(name, e.Location));
						Console.ResetColor();
					}
					if (result.Errors.Count == 0)
					{
						try
						{
							File.WriteAllBytes(name + ".asm", result.Bytes);
							Console.WriteLine();
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Compile successfully completed!");
							Console.ResetColor();
						}
						catch (Exception)
						{
							Console.WriteLine("Error 0: \"Couldn't write to file\" at 0");
						}
					}
				}
			}
			else {
				Console.WriteLine("Error 0: \"File does not exist\" at 0");
			}
		}
		static string WriteLoc(string name, int location)
		{
			string[] lines = File.ReadAllLines(name);
			string spac = "";
			int loc = location; loc++; int x = 0; int sl = 0;
			string lin = "";
			foreach (string line in lines)
			{
				loc -= line.Length;
				if (loc <= 0)
				{
					lin = line;
					sl = x;
					loc += line.Length;
					break;
				}
				x++;
			}
			for (int i = 2; i < loc; i++)
			{
				spac += " ";
			}
			return "(" + sl + "," + loc + "):" + Environment.NewLine + Environment.NewLine + lin.Replace("\t", " ") + Environment.NewLine + spac + "^^^";
		}
	}
}