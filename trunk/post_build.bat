if exist "%2tmp" goto tmp_exists
mkdir "%2tmp"

:tmp_exists
if not exist "C:\Program Files\Microsoft\ILMerge\ILMerge.exe" goto no_ilmerge_exists
"C:\Program Files\Microsoft\ILMerge\ILMerge.exe" /out:"%2tmp\GeekTool.exe" "%2GeekTool.exe" "%2Common.dll"
move /Y "%2tmp\GeekTool.exe" "%2GeekTool.exe"

:no_ilmerge_exists
rmdir /S /Q "%2tmp"
del "%2Common.dll"

if not exist "c:\utility\7-zip\7z.exe" goto end
"c:\utility\7-zip\7z.exe" u -tzip -mx9 "%2GeekTool_bin.zip" "%2ChangeLog.txt" "%2Errata.txt" "%2GeekTool.exe" "%2gpl.txt" "%2Readme.txt" "%2GeekTool.exe.config"
"c:\utility\7-zip\7z.exe" u -tzip -mx9 "%2GeekTool_src.zip" -r "%1*.*" -x!"%1bin\" -x!"%1Archives\" -x!"%1obj\" -x!"%1.svn\" -x!"%1*.user" -x!"%1*.NoLoad" -x!"%1*.suo"
:end