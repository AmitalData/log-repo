SETLOCAL enabledelayedexpansion
SET NumberErrors=0
SET TotalErrors

cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\Amital.DevOps\Logitude\AngularModules\AngularModules


FOR /L %%A IN (1,1,1) DO (  
    --------------------------------------------------SpotRate-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud"  --params.QuoteParams.Direction="Export" --params.QuoteParams.TransportMode="A"  --params.QuoteParams.ShipmentType="" --params.QuoteParams.QuoteType="SpotRate" --suite=login,NewQuote > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Export-Air-SpotRate"

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud"  --params.QuoteParams.Direction="Import" --params.QuoteParams.TransportMode="O"  --params.QuoteParams.ShipmentType="FCL" --params.QuoteParams.QuoteType="SpotRate" --suite=login,NewQuote > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Import-Ocean-FCL-SpotRate"

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.QuoteParams.Direction="Domestic" --params.QuoteParams.TransportMode="I"  --params.QuoteParams.ShipmentType="LTL" --params.QuoteParams.QuoteType="SpotRate" --suite=login,NewQuote > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Domestic-Inland-LTL-SpotRate"

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.QuoteParams.Direction="Drop" --params.QuoteParams.TransportMode="A"  --params.QuoteParams.ShipmentType="" --params.QuoteParams.QuoteType="SpotRate" --suite=login,NewQuote > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Drop-Air-SpotRate" 

    --------------------------------------------------RoutingRate-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.QuoteParams.Direction="Export" --params.QuoteParams.TransportMode="O"  --params.QuoteParams.ShipmentType="LCL" --params.QuoteParams.QuoteType="RoutingRate" --suite=login,NewQuote > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Export-Air-RoutingRate"

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.QuoteParams.Direction="Import" --params.QuoteParams.TransportMode="I"  --params.QuoteParams.ShipmentType="FTL" --params.QuoteParams.QuoteType="RoutingRate" --suite=login,NewQuote > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Import-Inland-LTL-RoutingRate"

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.QuoteParams.Direction="Domestic" --params.QuoteParams.TransportMode="A"  --params.QuoteParams.ShipmentType="" --params.QuoteParams.QuoteType="RoutingRate" --suite=login,NewQuote > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Domestic-Air-RoutingRate" 
  
    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.QuoteParams.Direction="Drop" --params.QuoteParams.TransportMode="O"  --params.QuoteParams.ShipmentType="FCL" --params.QuoteParams.QuoteType="RoutingRate" --suite=login,NewQuote > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Drop-Ocean-FCL-RoutingRate" 

    --------------------------------------------------Activities-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.CRM.CRMType="activity" --params.CRM.ActivityType="task" --suite=login,CRM > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Task"

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.CRM.CRMType="activity" --params.CRM.ActivityType="call" --suite=login,CRM > D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "PhoneCall"

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.CRM.CRMType="activity" --params.CRM.ActivityType="appoint" --suite=login,CRM >D:\E2ETeamAyman\Cloud\prot.log 2>&1
    CALL :CheckError "Appointment"

     -------------------------------------------------- Opportunities-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="cloudStaging" --params.Team="aymancloud" --params.CRM.CRMType="opportunity" --suite=login,CRM > D:\E2ETeamAyman\prot.log 2>&1
    CALL :CheckError "opportunity"

)
cd /
cd C:\Automation e2e\TeamAyman\Cloud
>test.txt echo Errors in : %TotalErrors%
>>test.txt echo Total Errors :%NumberErrors% 

IF %NumberErrors% NEQ 0 ( 
  exit 1
)
Pause

SETLOCAL
:CheckError
    set /a count=0
    cd /
    cd windows
    c:
    cd C:\Automation e2e\TeamAyman\Cloud\screenshots

    IF EXIST images (
        cd /
        cd windows
        c:
        cd C:\Automation e2e\TeamAyman\Cloud\screenshots\images

        for %%x in (*.png) do set /a count+=1
        set /A NumberErrors=count
    )
    cd /
    cd windows
    c:
    cd C:\Program Files (x86)\Jenkins\workspace\Amital.DevOps\Logitude\AngularModules\AngularModules

goto:eof
