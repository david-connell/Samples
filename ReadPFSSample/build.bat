@echo on
"%1..\NuGet.exe" pack "%1ReadPFS\TQC.PFSWrapper.nuspec" -Version 1.0.1
echo Hi '%1'
del "%1Build\*.nupkg"
xcopy "*.nupkg" "%1Build\" /Y
xcopy "*.nupkg" D:\NugetFeed\ /Y
del "*.nupkg"
