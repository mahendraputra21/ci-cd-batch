-------------------------------------------------------------------------------------------
For setting up your mentifi-deployment ON DEV SERVER, just follow these steps :
-------------------------------------------------------------------------------------------

a. Set your source_code_path, source_path, and destination_path on every bat process
   for example:
    set "auth_source_code_path=D:\auth-server\auth-server\auth-server\src\Hub3c.AuthServer"
    set "source_path=D:\mentifi-deployment\DEPLOYMENT\1.Publish-Hub3cAuthService-Dev"
    set "destination_path=\\20.213.161.174\c$\inetpub\wwwroot\TestHub3cAuthService-Dev"

b. You need to connect with VPN GS ID first 

c. Run CI_CD_Mentifi_Dev_Server.bat

---------------------------------------------------------
CI/CD FOR DEVELOPER MENTIFI
---------------------------------------------------------
Please choose an option to Deploy Mentifi service or apps:

1. AuthService-Dev

2. BillingEngine-Dev

3. WCFService-Dev

4. SignalR-Dev

5. WebUI-Dev

Enter your choice:

d. just choose what you want to deploy mentifi services.

notes: 
- This just for Dev Server(Staging) only
- For WCFService-Dev, SignalR-Dev,  WebUI-Dev you need to build first with visual studio