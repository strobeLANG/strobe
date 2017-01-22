using System.IO;
using System.Text;
public class ConsoleWriter : TextWriter
{
	private Gtk.TextBuffer textbox;
	public ConsoleWriter(Gtk.TextBuffer textbox)
	{
		this.textbox = textbox;
	}

	public override void Write(char value)
	{
		textbox.Text += value;
	}

	public override void Write(string value)
	{
		textbox.Text += value;
	}

	public override Encoding Encoding
	{
		get { return Encoding.ASCII; }
	}
}