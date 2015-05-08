@echo on
"%1..\..\NuGet.exe" pack "%1TQC.USBDevice\TQC.USBDevice.nuspec" -Version 1.3.5
echo '%1'
del "%1Build\*.nupkg"
xcopy "*.nupkg" "%1Build\" /Y
xcopy "*.nupkg" D:\NugetFeed\ /Y
del "*.nupkg"
