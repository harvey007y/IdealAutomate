rem need above blank line because there is garbage otherwise at start of batch file for some reason
rem I had to delete environmental variable Platform that had value of BPC and reboot to get this to work because it was overridding something
SET myFILE=%1



  "nuget.exe restore %myFILE%"
  "C:\Program Files (x86)\MSBuild\14.0\Bin\MSbuild.exe" /p:Configuration=Debug %myFILE%
  echo "!!!!!! %myFILE% has completed !!!!!!"
  pause
  