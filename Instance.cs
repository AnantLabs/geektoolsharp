using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GeekTool
{
	[XmlRoot("instance")]
	public class Instance
	{
		public Instance()
		{
		}

		private string name = string.Empty;
		private string fileName = string.Empty;
		private string fileArgs = string.Empty;
		private int xPos = 0;
		private int yPos = 0;
		private int width = 300;
		private int height = 300;
		private int opacity = 100;
		private string backgroundColor = "255,255,255";
		private string fontColor = "0,0,0";
		private string fontFamily = "Courier New";
		private int fontSize = 10;
		private int timerInterval = 2000;
		private bool isLocked = false;
		private string displayRegex = string.Empty;
		private bool isRegexCaseSensitive = false;
		private string displayTemplate = string.Empty;
		private string displayCharsToReplaceRegex = string.Empty;

		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		[XmlAttribute("fileName")]
		public string FileName
		{
			get 
			{ 
				return fileName; 
			}
			set 
			{ 
				fileName = value; 
			}
		}

		[XmlAttribute("fileArgs")]
		public string FileArgs
		{
			get
			{
				return fileArgs;
			}
			set
			{
				fileArgs = value;
			}
		}

		[XmlAttribute("xPos")]
		public int X
		{
			get
			{
				int val = xPos;

				if (val < 0)
				{
					val = SystemInformation.WorkingArea.Width + val;
				}

				return val;
			}
			set
			{
				xPos = value;
			}
		}

		[XmlAttribute("yPos")]
		public int Y
		{
			get
			{
				int val = yPos;

				if (val < 0)
				{
					val = SystemInformation.WorkingArea.Width + val;
				}

				return val;
			}
			set
			{
				yPos = value;
			}
		}

		[XmlAttribute("width")]
		public int Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		[XmlAttribute("height")]
		public int Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
			}
		}

		[XmlAttribute("opacity")]
		public int Opacity
		{
			get
			{
				return opacity;
			}
			set
			{
				opacity = value;
			}
		}

		[XmlAttribute("backgroundColor")]
		public string BackgroundColor
		{
			get
			{
				return backgroundColor;
			}
			set
			{
				backgroundColor = value;
			}
		}

		[XmlAttribute("fontColor")]
		public string FontColor
		{
			get
			{
				return fontColor;
			}
			set
			{
				fontColor = value;
			}
		}

		[XmlAttribute("fontFamily")]
		public string FontFamily
		{
			get
			{
				return fontFamily;
			}
			set
			{
				fontFamily = value;
			}
		}

		[XmlAttribute("fontSize")]
		public int FontSize
		{
			get
			{
				return fontSize;
			}
			set
			{
				fontSize = value;
			}
		}

		[XmlAttribute("timerInterval")]
		public int TimerInterval
		{
			get
			{
				return timerInterval;
			}
			set
			{
				if (value > 2000)
				{
					timerInterval = value;
				}
			}
		}

		[XmlAttribute("isLocked")]
		public bool IsLocked
		{
			get
			{
				return isLocked;
			}
			set
			{
				isLocked = value;
			}
		}

		[XmlAttribute("displayRegex")]
		public string DisplayRegex
		{
			get
			{
				return displayRegex;
			}
			set
			{
				displayRegex = value;
			}
		}

		[XmlAttribute("isRegexCaseSensitive")]
		public bool IsRegexCaseSensitive
		{
			get
			{
				return isRegexCaseSensitive;
			}
			set
			{
				isRegexCaseSensitive = value;
			}
		}

		[XmlAttribute("displayCharsToReplaceRegex")]
		public string DisplayCharsToReplaceRegex
		{
			get
			{
				return displayCharsToReplaceRegex;
			}
			set
			{
				displayCharsToReplaceRegex = value;
			}
		}

		[XmlAttribute("displayTemplate")]
		public string DisplayTemplate
		{
			get
			{
				return displayTemplate;
			}
			set
			{
				displayTemplate = value;
			}
		}
	}
}
