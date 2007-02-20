using System;
using System.Drawing;
using System.Configuration;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeekTool
{
	static class Program
	{
		private static List<Instance> instances = new List<Instance>();
		private static string logFileName = "log.txt";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// Set-up some application stuff.
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			try
			{
				// Grab our application settings.
				AppSettingsReader appSettings = new AppSettingsReader();
				logFileName = (string)appSettings.GetValue(Constants.LogFileName, typeof(string));

				// Get our instance-specific settings.
				instances = (List<Instance>)ConfigurationManager.GetSection(Constants.Instances);

				if (instances == null 
					|| instances.Count == 0)
				{
					throw new InvalidSettingsException("Settings weren't set-up correctly.");
				}

				// Show the instances specified.
				foreach (Instance instance in instances)
				{
					Main main = new Main(instance, logFileName);
					main.Show();
				}
			}
			catch (Exception ex)
			{
				Instance instance = new Instance();
				instance.Width = 300;
				instance.Height = 300;
				instance.X = 0;
				instance.Y = 0;

				instance.BackgroundColor = string.Format("{0},{1},{2}",
					Color.White.R,
					Color.White.G,
					Color.White.B);

				instance.FontColor = string.Format("{0},{1},{2}",
					Color.Red.R,
					Color.Red.G,
					Color.Red.B);

				instance.FontFamily = new Font(FontFamily.GenericMonospace, 10).ToString();
				
				Main main = new Main(instance, logFileName);
				main.WriteToScreen(ex.Message);
				main.Show();
			}

			// Start up the application.
			Application.Run();
		}
	}
}