SETLOCAL enabledelayedexpansion
SET NumberErrors=0
SET TotalErrors

cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\2019.R3.DevOps\Logitude\AngularModules\AngularModules


FOR /L %%A IN (1,1,1) DO (  

    --------------------------------------------------Direct-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="D" --params.ShipParams.Direction="Export" --params.ShipParams.TransportMode="A" --params.ShipParams.ShipmentType="" --suite=login,NewShipment >D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Export-Air-Direct"


    cmd /c call npm run e2e -- --params.Env="prod"  --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="D" --params.ShipParams.Direction="Import" --params.ShipParams.TransportMode="O" --params.ShipParams.ShipmentType="FCL" --suite=login,NewShipment> D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Import-Ocean-FCL-Direct"

    cmd /c call npm run e2e -- --params.Env="prod"  --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="D" --params.ShipParams.Direction="Domestic" --params.ShipParams.TransportMode="I" --params.ShipParams.ShipmentType="FTL" --suite=login,NewShipment > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Domestic-Inland-FTL-Direct"

    cmd /c call npm run e2e -- --params.Env="prod"  --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="D" --params.ShipParams.Direction="Drop" --params.ShipParams.TransportMode="I" --params.ShipParams.ShipmentType="LTL" --suite=login,NewShipment > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Drop-Inland-LTL-Direct"
    --------------------------------------------------Houses-------------------------------------------------------------------------------------------------
    cmd /c call npm run e2e -- --params.Env="prod"  --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="H" --params.ShipParams.Direction="Export" --params.ShipParams.TransportMode="A" --params.ShipParams.ShipmentType="" --suite=login,NewShipment > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Export-Air-House"

    cmd /c call npm run e2e -- --params.Env="prod"  --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="H" --params.ShipParams.Direction="Import" --params.ShipParams.TransportMode="O" --params.ShipParams.ShipmentType="FCL" --suite=login,NewShipment> D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Import-Ocean-FCL-House"

    cmd /c call npm run e2e -- --params.Env="prod"  --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="H" --params.ShipParams.Direction="Domestic" --params.ShipParams.TransportMode="O" --params.ShipParams.ShipmentType="LCL" --suite=login,NewShipment> D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Domestic-Ocean-LCL-House"

    cmd /c call npm run e2e -- --params.Env="prod"  --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="H" --params.ShipParams.Direction="Drop" --params.ShipParams.TransportMode="I" --params.ShipParams.ShipmentType="LTL" --suite=login,NewShipment > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Drop-Inland-LTL-House"
    --------------------------------------------------Master-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="M" --params.ShipParams.Direction="Export" --params.ShipParams.TransportMode="A" --params.ShipParams.ShipmentType="" --suite=login,NewShipment > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Export-Air-Master"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="M" --params.ShipParams.Direction="Import" --params.ShipParams.TransportMode="O" --params.ShipParams.ShipmentType="FCL" --suite=login,NewShipment> D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Import-Ocean-FCL-Master"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="M" --params.ShipParams.Direction="Domestic" --params.ShipParams.TransportMode="O" --params.ShipParams.ShipmentType="LCL" --suite=login,NewShipment> D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Domestic-Ocean-LCL-Master"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="M" --params.ShipParams.Direction="Drop" --params.ShipParams.TransportMode="I" --params.ShipParams.ShipmentType="LTL" --suite=login,NewShipment> D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Drop-Inland-LTL-Master"

    --------------------------------------------------SpotRate-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman"  --params.QuoteParams.Direction="Export" --params.QuoteParams.TransportMode="A"  --params.QuoteParams.ShipmentType="" --params.QuoteParams.QuoteType="SpotRate" --suite=login,NewQuote > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Export-Air-SpotRate"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman"  --params.QuoteParams.Direction="Import" --params.QuoteParams.TransportMode="O"  --params.QuoteParams.ShipmentType="FCL" --params.QuoteParams.QuoteType="SpotRate" --suite=login,NewQuote > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Import-Ocean-FCL-SpotRate"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.QuoteParams.Direction="Domestic" --params.QuoteParams.TransportMode="I"  --params.QuoteParams.ShipmentType="LTL" --params.QuoteParams.QuoteType="SpotRate" --suite=login,NewQuote > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Domestic-Inland-LTL-SpotRate"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.QuoteParams.Direction="Drop" --params.QuoteParams.TransportMode="A"  --params.QuoteParams.ShipmentType="" --params.QuoteParams.QuoteType="SpotRate" --suite=login,NewQuote > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Drop-Air-SpotRate" 

    --------------------------------------------------RoutingRate-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.QuoteParams.Direction="Export" --params.QuoteParams.TransportMode="O"  --params.QuoteParams.ShipmentType="LCL" --params.QuoteParams.QuoteType="RoutingRate" --suite=login,NewQuote > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Export-Air-RoutingRate"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.QuoteParams.Direction="Import" --params.QuoteParams.TransportMode="I"  --params.QuoteParams.ShipmentType="FTL" --params.QuoteParams.QuoteType="RoutingRate" --suite=login,NewQuote > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Import-Inland-LTL-RoutingRate"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.QuoteParams.Direction="Domestic" --params.QuoteParams.TransportMode="A"  --params.QuoteParams.ShipmentType="" --params.QuoteParams.QuoteType="RoutingRate" --suite=login,NewQuote > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Domestic-Air-RoutingRate" 
  
    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.QuoteParams.Direction="Drop" --params.QuoteParams.TransportMode="O"  --params.QuoteParams.ShipmentType="FCL" --params.QuoteParams.QuoteType="RoutingRate" --suite=login,NewQuote > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Drop-Ocean-FCL-RoutingRate" 

    --------------------------------------------------Activities-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.CRM.CRMType="activity" --params.CRM.ActivityType="task" --suite=login,CRM > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Task"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.CRM.CRMType="activity" --params.CRM.ActivityType="call" --suite=login,CRM > D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "PhoneCall"

    cmd /c call npm run e2e -- --params.Env="prod" --params.Team="ayman" --params.CRM.CRMType="activity" --params.CRM.ActivityType="appoint" --suite=login,CRM >D:\E2ETeamAyman\Prod\prot.log 2>&1
    CALL :CheckError "Appointment"

)
cd /
cd C:\Automation e2e\TeamAyman\Prod
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
    cd C:\Automation e2e\TeamAyman\prod\screenshots

    IF EXIST images (
        cd /
        cd windows
        c:
        cd C:\Automation e2e\TeamAyman\Prod\screenshots\images

        for %%x in (*.png) do set /a count+=1
        set /A NumberErrors=count
    )
    cd /
    cd windows
    c:
    cd C:\Program Files (x86)\Jenkins\workspace\2019.R3.DevOps\Logitude\AngularModules\AngularModules

goto:eof
