GeekTool
Version 0.5.2.0

GeekTool is Windows clone of a Mac OS X utility of the same name (http://projects.tynsoe.org/en/geektool/). 
It takes the output from a command-line program and displays it on the desktop.

The source code for this program is available under the GPL; a copy of the license should be distributed with the executable. 
The source can be downloaded from http://www.longueur.org/software/ or http://code.google.com/p/geektoolsharp/.

There are no warranties, implicit or otherwise, included with this software. 
Run at your own risk. And read the source to send me suggestions or improvements.

Most of the settings in the GeekTool.exe.config file are self-explanatory, but I will go through a quick rundown anyway.

xPos
	The position along the horizontal axis of the content. Can be a negative number.

yPos
	The position along the vertical axis of the content. Can be a negative number.

width
	Width of the content. Content will wrap if longer than width.

height
	Height of content.

opacity
	How transparent all of the content should be (this includes the text).

backgroundColor
	Color of the background. Specified as RGB. If "255,0,255" is used, then the background is transparent.

fontColor
	Color of the text. Specified as RGB.

fontFamily
	Name of the font for the text.

fontSize
	Size of the font.

timerInterval
	How often the content should refresh (in milliseconds). So, 5000 milliseconds = 5 seconds. The minimum value allowed is 2000.

fileName
	Name of the executable file that should be run.

fileArgs
	Any arguments for the file that should be run.

logFileName
	File name of the log that is written to. For debugging purposes only.

lockPosition
	Whether or not the content should be "locked" on the desktop. This prevents the content from being moved and also always sets it behind every other window.

displayRegex
	The regular expression that determines what is displayed on the screen. If explicit groups are specified, they can be referenced in the template (below).

displayTemplate
	The template used to output the content. If no template is specified, all content (that matches the regex above, if specified) is displayed. Explicit groups can also be specified which reference the groups in the regex above. And "{0}" can be used to specify the whole match from the regex.

isRegexCaseSensitive
	Specifies whether the regex is case-sensitive or not.

displayCharsToReplaceRegex
	Characters that should always be taken out of the content.