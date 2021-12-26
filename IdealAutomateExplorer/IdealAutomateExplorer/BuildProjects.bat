rem need above blank line because there is garbage otherwise at start of batch file for some reason
rem I had to delete environmental variable Platform that had value of BPC and reboot to get this to work because it was overridding something
SET myFILE=%1

SET myBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe

  "nuget.exe restore %myFILE%"
  echo %myFile%
  for /f "tokens=*" %%i in ('%myBuild%') do set RESULT=%%i
 "%RESULT%" /p:Configuration=Debug %myFILE%
  echo "!!!!!! %myFILE% has completed !!!!!!"
  pause
  