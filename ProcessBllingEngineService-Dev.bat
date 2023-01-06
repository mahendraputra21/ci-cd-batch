@echo off
setlocal enableextensions enabledelayedexpansion

set "bill_source_code_path=D:\hub3c-billing-engine\src\Hub3c.BillingEngine.Api"
set "source_path=D:\mentifi-deployment\DEPLOYMENT\2.Publish-Hub3cBillingEngine-Dev"
set "destination_path=\\20.213.161.174\c$\inetpub\wwwroot\Hub3cBillingEngine-Dev"

echo Stopping IIS on vm-mentifi-development...
sc \\20.213.161.174 stop W3SVC
echo IIS stopped on vm-mentifi-development.
echo:

:start
echo Publishing %1 on vm-mentifi-development...
cd /d %bill_source_code_path%
echo Running dotnet Release...
echo:
dotnet build -c Release
echo Running dotnet Publish...
echo:
rmdir /s /q %source_path%
dotnet publish -c Release -o %source_path%
echo:

echo Copying file from %source_path% to %destination_path%

robocopy "%source_path%" "%destination_path%" /MIR /R:10 /W:10 /XF %source_path%\web.config /XO

echo The files have been copied from %source_path% to %destination_path%.
echo:

echo Starting IIS on vm-mentifi-development...
sc \\20.213.161.174 start W3SVC
echo IIS strated on vm-mentifi-development.

goto end

:error
echo An error occurred....
echo Please check the log file for more information.

:end
pause
