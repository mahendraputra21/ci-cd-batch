@echo off
setlocal enableextensions enabledelayedexpansion

set "source_path=D:\mentifi-deployment\DEPLOYMENT\3.Publish-Hub3cService-Dev"
set "destination_path=\\20.213.161.174\c$\inetpub\wwwroot\Hub3cUI-Dev"

echo Stopping IIS on vm-mentifi-development...
sc \\20.213.161.174 stop W3SVC
echo IIS stopped on vm-mentifi-development.
echo:

:start
echo Copying file from %source_path% to %destination_path%

robocopy "%source_path%" "%destination_path%" /MIR /R:10 /W:10 /XF %source_path%\web.config /XD "%destination_path%\NLog (do not replace when deploy)" /XO

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
