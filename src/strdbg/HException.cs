using System;
using System.Text;
using System.Text.RegularExpressions;

namespace strdbg
{
	public partial class HException : Gtk.Dialog
	{
		public HException(Exception e)
		{
			Build();
			text.Buffer.Text = @"=== EXCEPTION HAS OCCURRED ===
Exception Type: " + e.GetType() + @"
Exception Message: " + e.Message + @"
Exception Code:
" + SpliceText(Convert.ToBase64String(Encoding.UTF8.GetBytes(e.ToString())),14 * 3) + @"

Please report this ASAP at:
https://github.com/mihail-mojsoski/Strobe/issues

";
		}

		public static string SpliceText(string text, int lineLength)
		{
			var x = Regex.Replace(text, "(.{2})", "$1" + "\t");
			return Regex.Replace(x, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		protected void OnClose(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}
	}
}
