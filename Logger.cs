using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GeekTool
{
	public class Logger
	{
		string logFileName;
		LogLevelType logLevel = LogLevelType.Verbose;

		public Logger(string logFileName)
		{
			this.logFileName = logFileName;
		}

		public Logger(string logFileName, LogLevelType logLevel)
		{
			this.logFileName = logFileName;
			this.logLevel = logLevel;
		}

		/// <summary>
		/// Helper method to open the log file and write to it.
		/// </summary>
		/// <param name="content">Content to write to the file.</param>
		public void Log(string content)
		{
			Log(content, LogLevelType.ErrorsOnly);
		}

		/// <summary>
		/// Helper method to open the log file and write to it.
		/// </summary>
		/// <param name="content">Content to write to the file.</param>
		/// <param name="logLevelType">The log level+ that will write the content.</param>
		public void Log(string content, LogLevelType logLevelType)
		{
			// Append to our log file if we are the level specified or above.
			if (logLevel >= logLevelType)
			{
				using (StreamWriter sw = new StreamWriter(logFileName, true))
				{
					sw.WriteLine(DateTime.Now + ": " + content);
					sw.Flush();
				}
			}
		}
	}
}