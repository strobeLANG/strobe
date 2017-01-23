using StrobeVM.Firmware;
using Gtk;
using System.IO;

namespace strdbg
{
	public static class Debug
	{
		public static string Input;
		// I use this to make sure that someone actually has a file with this contents.
		static string initl = "\t\t\t\t\t\t\t" + (char)0x0 + (char)0xff;
		public static Kernel kernel;
		public static byte[] app;
		public static void Main(string[] args)
		{
			Input = initl;
			foreach (string s in args)
			{
				try
				{
					Input = File.ReadAllText(s);
				}
				catch(System.Exception)
				{
					System.Console.Write("Couldn't load. RIP");
				}
			}
			if (Input == initl)
			{
				Input = @"#include <bioslib.str>
#include <math.str>
namespace MyProgram
{
    /*
     * The start of your application.
     */
	function Main()
	{
		System.Write(""Hello World\n"");
		System.Exit(0);
	}
}
";
			}
			kernel = new Kernel(1024*1024);
			Application.Init();
			try
			{
				new MainWindow().Show();
				Application.Run();
			}
			catch (System.Exception e)
			{
				new HException(e).Show();
				Application.Run();
			}
		}
	}
}
