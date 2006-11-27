using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTool
{
	class InvalidSettingsException : Exception
	{
		public InvalidSettingsException(string message)
			: base(message)
		{
		}
	}
}
