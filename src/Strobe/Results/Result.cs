using System.Collections.Generic;
namespace Strobe
{
	public class Result
	{
		public List<Warning> Warnings = new List<Warning>();
		public List<Error> Errors = new List<Error>();
	}
	public class Warning : Info { }
	public class Error : Info {	}
	public class Info
	{
		public string Value;
		public int Code;
		public int Location;
	}
}
