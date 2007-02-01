using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace GeekTool
{
	public partial class Main : Form
	{
		// Constants.
		private const string firstGroupConst = "{0}";
		private const int GWL_EXSTYLE = (-20);
		private const int WS_EX_TOOLWINDOW = 0x80;
		private const int WS_EX_APPWINDOW = 0x40000;
		private const int WM_QUERYENDSESSION = 0x0011;
		private const int WM_ENDSESSION = 0x0016;

		// Import some Win32 API methods.
		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("User32.dll")]
		public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

		[DllImport("User32.dll")]
		static extern int SetParent(int hWndChild, int hWndNewParent);

		// Member variables to store some in-house stuff.
		private bool isMouseDown = false;
		private Point downLocation;
		private Point downMousePosition;
		private Logger logger;
		private Timer timer = new Timer();
		private Regex explicitGroupsRegex = new Regex(@"(\{[^0]+\})", RegexOptions.Compiled);

		// Settings.
		private Instance instance;
		private string logFileName = "log.txt";

		// The regular expressions to use. We only want to create these once.
		private Regex displayRegex;
		private Regex displayCharsToReplaceRegex;
		
		// Process-related objects.
		private ProcessStartInfo processStartInfo;
		private Process process;
		private object lockObject = new object();

		/// <summary>
		/// The main access point of the program.
		/// </summary>
		public Main(Instance instance, string logFileName)
		{
			try
			{
				// Initialize VS designer stuff.
				InitializeComponent();

				// Make sure that our form doesn't show up while a user is ALT-TABing.
				SetWindowLong(this.Handle, GWL_EXSTYLE, (GetWindowLong(this.Handle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);

				// Settings.
				this.instance = instance;
				this.logFileName = logFileName;

				// Wire up when the form gets activated..
				this.Activated += new EventHandler(Main_Activated);

				// Wire up our mouse events.
				textLabel.MouseMove += new MouseEventHandler(mouseMove);
				textLabel.MouseUp += new MouseEventHandler(mouseUp);
				textLabel.MouseClick += new MouseEventHandler(mouseClick);

				// Set our settings.
				setSettings();

				// If the timer is sufficient, wire it up and start it. Otherwise throw a big, fat error.
				if (instance.TimerInterval > 0)
				{
					startProcess();
					startTimer();
				}
				else
				{
					throw new Exception("The timer interval must be higher than 0.");
				}
			}
			// Catch any errors that we get from missing settings.
			catch (InvalidSettingsException ex)
			{
				// Cleanup our events and stop some timers.
				cleanup();

				// Set our size to some default.
				Size size = new Size(300, 300);

				// Set the rest of our form values to some nice defaults.
				base.Location = new Point(0, 0);
				base.Size = size;
				textLabel.Size = size;

				base.BackColor = Color.White;
				base.ForeColor = Color.Red;
				base.Font = new Font(FontFamily.GenericMonospace, 10);
				
				// Write our error message to the screen 
				// (luckily the settings reader gives us some readable errors).
				WriteToScreen(ex.Message);
			}
			// Catch all other errors here and write them to the screen and to the log file.
			catch (Exception ex)
			{
				// Write the exception to the log file and to the screen if possible.
				string exception = ex.Message + ex.StackTrace;

				WriteToScreen(exception);

				if (logger != null)
				{
					logger.Log(exception);
				}
			}
		}

		#region set-up
		/// <summary>
		/// Take our settings and implement them.
		/// </summary>
		private void setSettings()
		{
			// Setup the logger.
			this.logger = new Logger(this.logFileName);

			// Determine our location by the X and Y position.
			Point location = new Point(instance.X, instance.Y);

			// If we want to lock, call a Win32 API to do it.
			if (instance.IsLocked)
			{
				int pWnd = FindWindow("Progman", null);
				int tWnd = this.Handle.ToInt32();

				SetParent(tWnd, pWnd);
			}

			// Setup our main regex. 
			if (!string.IsNullOrEmpty(instance.DisplayRegex))
			{
				if (instance.IsRegexCaseSensitive)
				{
					displayRegex = new Regex(instance.DisplayRegex, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.ExplicitCapture);
				}
				else
				{
					displayRegex = new Regex(instance.DisplayRegex, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
				}
			}

			// Setup the regex of the characters we want to get rid of.
			if (!string.IsNullOrEmpty(instance.DisplayCharsToReplaceRegex))
			{
				if (instance.IsRegexCaseSensitive)
				{
					displayCharsToReplaceRegex = new Regex(instance.DisplayCharsToReplaceRegex, RegexOptions.Compiled);
				}
				else
				{
					displayCharsToReplaceRegex = new Regex(instance.DisplayCharsToReplaceRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
				}
			}

			// Setup the form.
			base.BackColor = General.ConvertStringToColor(instance.BackgroundColor);
			base.ForeColor = General.ConvertStringToColor(instance.FontColor);
			base.Font = new Font(instance.FontFamily, instance.FontSize);
			base.Location = location;

			// Appearantly WinForms can't be any smaller than 112x27.
			if (instance.Width < 112)
			{
				instance.Width = 112;
			}

			if (instance.Height < 27)
			{
				instance.Height = 27;
			}

			// Set our size.
			Size size = new Size(instance.Width, instance.Height);
			this.ClientSize = size;
			textLabel.Size = size;
		}
		#endregion

		#region WriteToScreen
		/// <summary>
		/// Writes the message to the form, overwriting any old output.
		/// </summary>
		/// <param name="message">The text to write.</param>
		public void WriteToScreen(string message)
		{
			WriteToScreen(message, false);
		}

		/// <summary>
		/// Writes the message to the form.
		/// </summary>
		/// <param name="message">The text to write.</param>
		/// <param name="append">Whether to append the message or not.</param>
		public void WriteToScreen(string message, bool append)
		{
			// Whether to append the output, or overwrite the old output.
			if (append)
			{
				textLabel.Text += message;
			}
			else
			{
				textLabel.Text = message;
			}
		}
		#endregion

		/// <summary>
		/// Starts the process which is generating the output.
		/// </summary>
		private void startProcess()
		{
			// Set the file information about the process.
			if (processStartInfo == null)
			{
				process = new Process();
				processStartInfo = new ProcessStartInfo();

				processStartInfo.FileName = instance.FileName;

				if (!string.IsNullOrEmpty(instance.FileArgs))
				{
					processStartInfo.Arguments = instance.FileArgs;
				}

				// These have to be set like this so we can get the output.
				processStartInfo.CreateNoWindow = true;
				processStartInfo.RedirectStandardError = true;
				processStartInfo.ErrorDialog = false;
				processStartInfo.RedirectStandardOutput = true;
				processStartInfo.UseShellExecute = false;

				// Set all of the information to our process.
				process.StartInfo = processStartInfo;
			}

			lock (lockObject)
			{
				try
				{
					process.Start();
					process.WaitForExit();

					// Grab the standard output from the process and write it.
					using (System.IO.StreamReader sr = process.StandardOutput)
					{
						string stdOutput = sr.ReadToEnd();

						if (!string.IsNullOrEmpty(stdOutput))
						{
							string output = parseOutput(stdOutput);
							WriteToScreen(output);
						}
					}

					// Grab the error output from the process and write it.
					using (System.IO.StreamReader sr = process.StandardError)
					{
						string stdError = sr.ReadToEnd();

						if (!string.IsNullOrEmpty(stdError))
						{
							WriteToScreen(stdError);
						}
					}
				}
				catch (Exception ex)
				{
					// Write the exception to the log file and screen.
					string exception = ex.Message + ex.StackTrace;

					WriteToScreen(exception);
					logger.Log(exception);
				}
			}
		}

		/// <summary>
		/// Parses the output from the process.
		/// </summary>
		/// <param name="stdOutput">Output that we are parsing.</param>
		/// <returns>A nicely formatted string.</returns>
		private string parseOutput(string stdOutput)
		{
			// Make sure there is some regex to parse the output with.
			if (!string.IsNullOrEmpty(instance.DisplayRegex)
				&& displayRegex != null)
			{
				StringBuilder output = new StringBuilder();
				string[] explicitGroups = { };

				// If we have a display template clean it up a little bit, 
				// and then grab the groups that are defined in the regex.
				if (!string.IsNullOrEmpty(instance.DisplayTemplate))
				{
					instance.DisplayTemplate = instance.DisplayTemplate.Replace("\\n", Environment.NewLine);

					// Only get the group names if we are using an explicit group 
					// in the template that is not '{0}'
					if (explicitGroupsRegex.IsMatch(instance.DisplayTemplate))
					{
						explicitGroups = displayRegex.GetGroupNames();
					}
				}

				// Loop over all the matches.
				foreach (Match match in displayRegex.Matches(stdOutput))
				{
					// If there are no or there is only one explicit group 
					// (there is always at least one if the regex is a match),
					// take the match and either use the default or the custom template.
					if (explicitGroups == null
						|| explicitGroups.Length <= 1)
					{
						// If there is no template just output everything with a newline at the end.
						if (string.IsNullOrEmpty(instance.DisplayTemplate))
						{
							string matchValue = displayCharsToReplaceRegex.Replace(match.Value, string.Empty);

							output.Append(matchValue);
							output.Append(Environment.NewLine);
						}
						// Use the template to replace '{0}' with the whole match.
						else
						{
							string matchValue = displayCharsToReplaceRegex.Replace(match.Value, string.Empty);

							string tmpOutput = instance.DisplayTemplate.Replace(firstGroupConst, matchValue);
							output.Append(tmpOutput);
						}
					}
					// The template has some explicit groups that aren't '{0}',
					// we should be nice and try to do what they want.
					else
					{
						string tmpOutput = instance.DisplayTemplate;
						string matchValue = string.Empty;

						// Loop over all of the explicit groups and replace any of 
						// the explicit groups in the template with their real values from the match.
						foreach (string name in explicitGroups)
						{
							matchValue = match.Groups[name].Value;

							if (!string.IsNullOrEmpty(matchValue))
							{
								matchValue = displayCharsToReplaceRegex.Replace(matchValue, string.Empty);

								tmpOutput = tmpOutput.Replace("{" + name + "}", matchValue);
							}
						}

						// As long as our result isn't the same as the template, let's keep it.
						if (!tmpOutput.Equals(instance.DisplayTemplate))
						{
							output.Append(tmpOutput);
						}
					}
				}

				// Return our output string.
				return output.ToString();
			}

			// Appearantly we are going to just return the standard out untouched.
			return stdOutput;
		}

		/// <summary>
		/// Start the timer.
		/// </summary>
		private void startTimer()
		{
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = instance.TimerInterval;
			timer.Enabled = true;
			timer.Start();
		}

		/// <summary>
		/// Stop the timer.
		/// </summary>
		private void stopTimer()
		{
			if (timer.Enabled)
			{
				timer.Enabled = false;
				timer.Stop();
				timer.Tick -= timer_Tick;
			}
		}

		/// <summary>
		/// Cleans up our wired up events and stops the main timer.
		/// </summary>
		private void cleanup()
		{
			// Stop the main timer.
			stopTimer();

			// Unregister all of our wired events.
			this.Activated -= Main_Activated;
			textLabel.MouseMove -= mouseMove;
			textLabel.MouseUp -= mouseUp;

			// Get rid of our process.
			lock (lockObject)
			{
				process.Dispose();
			}
		}

		#region events
		/// <summary>
		/// Event which gets fired when the form is activated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Main_Activated(object sender, EventArgs e)
		{
			// Send this bad boy to back of class.
#if !DEBUG
			this.SendToBack();
#endif
		}

		/// <summary>
		/// Event which gets fired when the timer interval elapses.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer_Tick(object sender, EventArgs e)
		{
			logger.Log(instance.Name + ":" + "Timer tick!");

			lock (lockObject)
			{
				logger.Log(instance.Name + ":" + "Start process!");
				startProcess();
			}
		}

		/// <summary>
		/// Event which gets fired when the exit menu item is selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			cleanup();
			Application.Exit();
		}

		/// <summary>
		/// Event which gets fired when the mouse moves over the form or textlabel.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Arguments.</param>
		private void mouseMove(object sender, MouseEventArgs e)
		{
			// Lock the form if this event gets fired.
			lock (this)
			{
				// We can safely ignore the rest if we are locked.
				if (!instance.IsLocked)
				{
					// Only worry about left button clicks.
					if (e.Button == MouseButtons.Left)
					{
						if (isMouseDown)
						{
							// Change our cursor.
							this.Cursor = Cursors.SizeAll;

							// Stop the main timer thread.
							stopTimer();

							// Figure out our location and update orm accordingly.
							Point mousePos = Form.MousePosition;
							Point location = downLocation;

							location.X = location.X + (mousePos.X - downMousePosition.X);
							location.Y = location.Y + (mousePos.Y - downMousePosition.Y);

							base.Location = location;

							// Output the location to the screen.
							string locationStr = string.Format("x: {0}; y: {1}",
								location.X,
								location.Y);

							WriteToScreen(locationStr);
						}
						else
						{
							downLocation = base.Location;
							downMousePosition = Form.MousePosition;
							isMouseDown = true;
						}
					}
					else
					{
						isMouseDown = false;
					}
				}
			}
		}

		/// <summary>
		/// Event which gets fired when the mouse click is up when on the form or textlabel.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Arguments.</param>
		private void mouseUp(object sender, MouseEventArgs e)
		{
			lock (this)
			{
				if (!instance.IsLocked)
				{
					Point location = base.Location;

					if (location.X != instance.X
						|| location.Y != instance.Y)
					{
						instance.X = location.X;
						instance.Y = location.Y;

						// TODO: Save these values to app.config.
					}

					if (!timer.Enabled)
					{
						timer.Enabled = true;
						timer.Start();

						WriteToScreen("Refreshing...");
					}

					this.Cursor = Cursors.Default;
				}
			}
		}

		/// <summary>
		/// Event which gets fired when the mouse clicks on the form or textlabel.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Arguments.</param>
		private void mouseClick(object sender, MouseEventArgs e)
		{
			// Show our context menu on right button clicks.
			if (e.Button == MouseButtons.Right)
			{
				contextMenuStrip.Show(Form.MousePosition.X, Form.MousePosition.Y);
			}
		}
		#endregion

		#region overrides
		/// <summary>
		/// Override for when the mouse moves over the form.
		/// </summary>
		/// <param name="e">Arguments.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Call our custom mouse move method.
			mouseMove(null, e);
			base.OnMouseMove(e);
		}

		/// <summary>
		/// Override for when the mouse click is up on the form.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Call our custom mouse up method.
			mouseUp(null, e);
			base.OnMouseUp(e);
		}

		/// <summary>
		/// Override for when the mouse clicks on the form.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			// Call our custom mouse click method.
			mouseClick(null, e);
			base.OnMouseClick(e);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_QUERYENDSESSION)
			{
				cleanup();
			}

			base.WndProc(ref m);
		}
		#endregion
	}
}