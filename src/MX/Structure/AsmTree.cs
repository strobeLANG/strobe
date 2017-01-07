using System.Collections.Generic;
using System;
using System.Text;

namespace MX
{
	public class AsmTree
	{
		public List<AsmBss> Variables;
		public List<AsmDat> Constants;
		public List<AsmTxt> SourceTxt;
		public Arch Architecture;
		public string Main;
		public byte[] save()
		{
			string o =  "section .bss";
			foreach(AsmBss ab in Variables)
			{
				o += Environment.NewLine + ab.get ();
			}
			o += Environment.NewLine + "section .data";
			foreach(AsmDat ad in Constants)
			{
				o += Environment.NewLine + ad.get ();
			}
			o += Environment.NewLine + "section .text";
			o += Environment.NewLine + "global _start";
			o += Environment.NewLine + "jmp " + Main;
			o += Environment.NewLine + "ret";
			foreach(AsmTxt at in SourceTxt)
			{
				o += Environment.NewLine + at.Value;
			}
			return Encoding.ASCII.GetBytes(o);
		}
	}
	public enum Arch
	{
		elf64 = 64,
		elf_i386 = 32,
	}
	public class AsmBss
	{
		
		public string Name;
		public Def Define;
		public string Size;
		public bool Override = false;
		public enum Def
		{
			DB,
			DW,
			DD,
			DQ,
			DT,
			DO,
			DY,
			DZ,
			EQU
		}
		public string get()
		{
			if (!Override)
				return Name + ": " + Define + " " + Size;
			else
				return Name;
		}

	}
	public class AsmDat
	{
		public string Name;
		public string Define;
		public string Size;
		public string Value;
		public bool Override = false;
		public enum Def
		{
			RESB,
			RESW,
			RESD,
			RESQ,
			REST,
			RESO,
			RESY,
			RESZ
		}
		public string get()
		{
			if (!Override)
				return Name + ": " + Define + " " + Value + "," + Size;
			else
				return Name;
		}
	}
	public class AsmTxt
	{
		public string Value;
	}
}

