using System.Text;
using System.Collections.Generic;

namespace Strobe
{
	public class CodeGenerator
	{
		Result Res = new Result();
		List<string> OtherPrep = new List<string>();
		List<string> Includes = new List<string>();
		ParseTree input;
		string output = "";
		public CodeGenerator (ParseTree input)
		{
			this.input = input;
			Generate();
		}
		void Generate()
		{
			foreach (string str in input.Preprocessor)
			{
				if (str?.Length > 0)
				PreProcess(str);
			}
		}
		void PreProcess(string s)
		{
			PreProcessorInst p = PreProcessorInst.None;
			string param = "";
			string content = "";
			bool isDone = false;
			int now = 0; char x;
			while (now < s.Length)
			{
				x = s[now];
				if (char.IsLetter(x))
				{
						string y = "";
						while (char.IsLetter(x) || now < s.Length)
						{
							y += x;
							now++;
							if (now<s.Length)
							x = s[now];
						}
						if (!isDone)
						{
							if (y == "include")
							{
								p = PreProcessorInst.Include;
								isDone = true;
							}
							if (y == "define")
							{
								p = PreProcessorInst.Define;
								isDone = true;
							}
						}
						else {
							param = y;
						}
				}
				if (x == '"' || x == '<')
				{
						string y = "";
						now++;
						while (x != '<' || x != '"' || now < s.Length)
						{
							y += x;
							now++;
							x = s[now];
						}
						now++;
						content = y;
				}
				now++;
			}
			System.Console.WriteLine("{0}:{1}({2})",p,content,param);
		}
		enum PreProcessorInst
		{
			Include,
			Define,
			None
		}
		public CodeGeneratorResult get()
		{
			return new CodeGeneratorResult
			{
				Errors = Res.Errors,
				Warnings = Res.Warnings,
				Code = Encoding.ASCII.GetBytes(output),
			};
		}
	}
}

