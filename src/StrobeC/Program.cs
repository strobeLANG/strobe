using System.IO;
using System;
using Strobe;
using strvmc;

namespace StrobeC
{
	class MainStrobeC
	{
		public static void Main(string[] args)
		{
            bool pauseEnd = false;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Compiling...");
			Console.ResetColor();
            foreach(string a in args)
            {
                if (a == "--pause")
                    pauseEnd = true;
            }
			if (args.Length > 0)
			{
				string name = args[0];
				string save = args[0] + ".dif";
				for (int i = 1; i < args.Length; i++)
				{
					if (args[i].StartsWith("--out:"))
					{
						save = args[i].TrimStart("--out:".ToCharArray());
					}
				}
				if (!File.Exists(name))
				{
					Console.WriteLine("Error 0: \"File does not exist\" at 0");
				}
				else {
					CompilerResult result = new CompilerResult ();
					string input = File.ReadAllText(name);
					try {
					result = new Compiler(input).compile();
					} catch(Exception e) {
						Console.WriteLine ("Error n: {0}",e.Message);
					}
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
							File.WriteAllBytes(save, result.Bytes);
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Compile successfully completed!");
							Console.ForegroundColor = ConsoleColor.DarkGray;
							Console.WriteLine("VM Runtime [1M]:\n");
							Console.ResetColor();
							StrobeVMC.Main(new string[]{"--1m",save});

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
            if(pauseEnd)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
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
