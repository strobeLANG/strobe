using System.IO;
public class ConsoleReader : TextReader
{
	private Gtk.Entry entry;
	public ConsoleReader(Gtk.Entry Entry)
	{
		entry = Entry;
	}

	public override int Read()
	{
		try
		{
			return int.Parse(entry.Text);
		}
		catch
		{
			return 0;
		}
	}

	public override string ReadLine()
	{
		try
		{
			return entry.Text;
		}
		catch
		{
			return "";
		}
	}
}