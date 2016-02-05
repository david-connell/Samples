@echo on

setLocal EnableDelayedExpansion
wmic DATAFILE WHERE name="D:\\TQC\\Samples\\LowLevelUSBComms\\TQC.USBDevice\\Build\\Release\\TQC.USBDevice.dll" get Version > temp.txt
type temp.txt> t1.txt


for /f %%a in (D:\TQC\Samples\LowLevelUSBComms\TQC.USBDevice\Build\Release\t1.txt) do (
set version=%%a
)

"%1..\..\NuGet.exe" pack "%1TQC.USBDevice\TQC.USBDevice.nuspec" -Version %version%
echo '%1'
del "%1Build\*.nupkg"
xcopy "*.nupkg" "%1Build\" /Y
xcopy "*.nupkg" D:\NugetFeed\ /Y
del "*.nupkg"
