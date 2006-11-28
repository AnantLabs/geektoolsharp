using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace GeekTool
{
	class General
	{
		public static Color ConvertStringToColor(string rgb)
		{
			string[] colors = rgb.Split(',');

			int r = int.Parse(colors[0].Trim());
			int g = int.Parse(colors[1].Trim());
			int b = int.Parse(colors[2].Trim());

			return Color.FromArgb(r, g, b);
		}
	}
}
