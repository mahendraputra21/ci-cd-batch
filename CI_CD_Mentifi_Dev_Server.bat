@echo off

:start
echo ---------------------------------------------------------
echo CI/CD FOR DEVELOPER MENTIFI
echo ---------------------------------------------------------
echo Please choose an option to Deploy Mentifi service or apps:
echo:

echo 1. AuthService-Dev
echo:
echo 2. BillingEngine-Dev
echo:
echo 3. WCFService-Dev
echo:
echo 4. SignalR-Dev
echo:
echo 5. WebUI-Dev
echo:

set /p choice=Enter your choice: 

if "%choice%" == "1" (

  echo You have chosen AuthService-Dev.
  call ProcessAuthService-Dev AuthService-Dev

) else if "%choice%" == "2" (

  echo You have chosen BillingEngine-Dev.
  call ProcessBllingEngineService-Dev BillingEngine-Dev

) else if "%choice%" == "3" (

  echo You have chosen WCFService-Dev.
  call ProcessWCFService-Dev WCFService-Dev

) else if "%choice%" == "4" (

  echo You have chosen SignalR-Dev.
  call ProcessSignalR-Dev SignalR-Dev

) else if "%choice%" == "5" (

  echo You have chosen WebUI-Dev.
  call ProcessWebUI-Dev WebUI-Dev

) else (
  cls
  echo Invalid choice...
  echo:
  goto start

)

pause
