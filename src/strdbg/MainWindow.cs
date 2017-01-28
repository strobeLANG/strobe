using System;
using System.Text;
using StrobeVM.DIF;
using strdbg;
using Gtk;
using System.Text.RegularExpressions;

public partial class MainWindow : Gtk.Window
{
	ConsoleReader com;
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		com = new ConsoleReader(CommandToSend);
		Editor.Buffer.Text = Debug.Input;
		Console.SetOut(new ConsoleWriter(Cons.Buffer));
		Console.SetIn(com);
	}

	public static string SpliceText(string text, int lineLength)
	{
		return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
	}

	void ShowMemory()
	{
		StringBuilder hex = new StringBuilder(Debug.kernel.mem.Ram.Length * 2);
		foreach (byte b in Debug.kernel.mem.Ram)
		{
			if (char.IsLetterOrDigit((char)b))
				hex.Append((char)b + " \t");
			else if (b != 0)
				hex.AppendFormat("{0:x2}\t", b);
			else
				hex.Append("..\t");
		}
		cODE.Buffer.Text = SpliceText(hex.ToString(), 3 * 16);
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		
		Application.Quit();
		a.RetVal = true;
	}

	void Update()
	{
        if (Debug.app == null)
        {
            return;
        }
		StringBuilder hex = new StringBuilder(Debug.app.Length * 2);
		foreach (byte b in Debug.app)
			hex.AppendFormat("{0:x2}\t", b);
		cODE.Buffer.Text = SpliceText(hex.ToString(), 3*16);
	}

	protected void OnResizeKernelButtonClicked(object sender, EventArgs e)
	{
		Debug.kernel = new StrobeVM.Firmware.Kernel((int)KernelSize.Value);
	}

	protected void OnCompileClicked(object sender, EventArgs e)
	{
		Strobe.CompilerResult res;
		try
		{
			ResultText.Buffer.Text = "===Build Started===\n";
			res = new Strobe.Compiler(Editor.Buffer.Text).compile();
			Debug.app = res.Bytes;
			foreach (Strobe.Error err in res.Errors)
			{
				ResultText.Buffer.Text += "Error: "+err.Value + " at " + err.Location + "\n";
			}
			foreach (Strobe.Warning war in res.Warnings)
			{
				ResultText.Buffer.Text += "Warning: " + war.Value + " at " + war.Location + "\n";
			}
			ResultText.Buffer.Text += "Errors:"+res.Errors.Count+"\nWarnings:"+res.Warnings.Count+"\n";
		}
		catch(Exception up)
		{
			ResultText.Buffer.Text +="Exception: " + up.Message + "\n";
		}
		ResultText.Buffer.Text += "===Build finished===\n";
		Update();
	}

	protected void OnRunClicked(object sender, EventArgs e)
	{
		try
		{
			OnResizeKernelButtonClicked(null, null);
			Debug.kernel.Start(new DIFFormat().Load(Debug.app));
			ResultText.Buffer.Text = "Code currently running.\nUse the Debugger and Console tabs.\n";
		}
		catch
		{
			ResultText.Buffer.Text = "You need to compile using the correct format.\n";
		}
	}

	protected void OneStep()
	{
		if (Debug.kernel.running.Count > 0)
			try
			{
				Debug.kernel.Step();
			}
			catch (Exception ex)
			{
				ResultText.Buffer.Text += "VM Exception: \"" + ex.Message + "\n\"";
			}
		else
		{
			ResultText.Buffer.Text += "Code isn't currently running (or is complete).\n";
		}
	}

	protected void StepUntilException()
	{
		while (Debug.kernel.running.Count > 0)
		{
			try
			{
				Debug.kernel.Step();
			}
			catch(Exception ex)
			{
				ResultText.Buffer.Text += "VM Exception: " + ex.Message + "\n";
				break;
			}
		}
		ResultText.Buffer.Text += "Code isn't currently running (or is complete).\n";
	}

	protected void StepUntilEnd()
	{
		while (Debug.kernel.running.Count > 0)
		{
			try
			{
				Debug.kernel.Step();

			}
			catch (Exception ex)
			{
				ResultText.Buffer.Text += "VM Exception: " + ex.Message + "\n";
			}
		}
		ResultText.Buffer.Text += "Code isn't currently running (or is complete).\n";
	}

	protected void StepOnce(object sender, EventArgs e)
	{
		OneStep();
	}

	protected void UntilError(object sender, EventArgs e)
	{
		StepUntilException();
	}

	protected void UntilEnd(object sender, EventArgs e)
	{
		StepUntilEnd();
	}

	protected void StopKernel(object sender, EventArgs e)
	{
		if (Debug.kernel.running.Count > 0)
		{
			ResultText.Buffer.Text += "Stopped running code.\n";
			Debug.kernel.Stop();
		}
		else
			ResultText.Buffer.Text += "Code isn't currently running (or is complete).\n";
	}

	protected void DumMem(object sender, EventArgs e)
	{
		ShowMemory();
	}
}
