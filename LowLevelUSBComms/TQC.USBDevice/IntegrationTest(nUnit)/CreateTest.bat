@echo off
rd NunitTest/s /q
md NunitTest
xcopy *.dll NunitTest /R /Y
copy __*.bat NunitTest\*.bat 


rem Copy over NUNIT test Code

Set _NUINT=C:\Program Files (x86)\NUnit 2.6.3\bin

xcopy "%_NUINT%\nunit*x86.*" NunitTest /R /Y
xcopy "%_NUINT%\Lib\*" NunitTest\Lib\* /R /Y
Set _NUINT=

rem 
rem Zip files
rem 

del TQC.USBDevice.NunitTest.7z
cd "NunitTest"
"C:\Program Files\7-Zip\7z.exe"  a "..\TQC.USBDevice.NunitTest.7z" -t7z -r  
cd ..


rem 
rem Clear up tempory files
rem 
rd NunitTest/s /q

