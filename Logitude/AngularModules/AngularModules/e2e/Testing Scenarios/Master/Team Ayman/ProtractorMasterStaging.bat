SETLOCAL enabledelayedexpansion
SET NumberErrors=0
SET TotalErrors

cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\LogitudeTestDevOps\Logitude\AngularModules\AngularModules


FOR /L %%A IN (1,1,1) DO (  
    --------------------------------------------------Direct-------------------------------------------------------------------------------------------------

    cmd /c call npm run e2e -- --params.Env="test_staging" --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="D" --params.ShipParams.Direction="Export" --params.ShipParams.TransportMode="A" --params.ShipParams.ShipmentType="" --suite=login,NewShipment >D:\E2ETeamAyman\prot.log 2>&1
    CALL :CheckError "Export-Air-Direct"

    --------------------------------------------------Houses-------------------------------------------------------------------------------------------------
    cmd /c call npm run e2e -- --params.Env="test_staging"  --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="H" --params.ShipParams.Direction="Import" --params.ShipParams.TransportMode="O" --params.ShipParams.ShipmentType="FCL" --suite=login,NewShipment> D:\E2ETeamAyman\prot.log 2>&1
    CALL :CheckError "Import-Ocean-FCL-House"

    --------------------------------------------------Master-------------------------------------------------------------------------------------------------
    cmd /c call npm run e2e -- --params.Env="test_staging" --params.Team="ayman" --params.ShipParams.ShipmentLevelCode="M" --params.ShipParams.Direction="Domestic" --params.ShipParams.TransportMode="O" --params.ShipParams.ShipmentType="LCL" --suite=login,NewShipment> D:\E2ETeamAyman\prot.log 2>&1
    CALL :CheckError "Domestic-Ocean-LCL-Master"

    --------------------------------------------------SpotRate-------------------------------------------------------------------------------------------------
    cmd /c call npm run e2e -- --params.Env="test_staging" --params.Team="ayman"  --params.QuoteParams.Direction="Export" --params.QuoteParams.TransportMode="A"  --params.QuoteParams.ShipmentType="" --params.QuoteParams.QuoteType="SpotRate" --suite=login,NewQuote > D:\E2ETeamAyman\prot.log 2>&1
    CALL :CheckError "Export-Air-SpotRate"

    --------------------------------------------------RoutingRate-------------------------------------------------------------------------------------------------
    cmd /c call npm run e2e -- --params.Env="test_staging" --params.Team="ayman" --params.QuoteParams.Direction="Domestic" --params.QuoteParams.TransportMode="A"  --params.QuoteParams.ShipmentType="" --params.QuoteParams.QuoteType="RoutingRate" --suite=login,NewQuote > D:\E2ETeamAyman\prot.log 2>&1
    CALL :CheckError "Domestic-Air-RoutingRate" 
)
cd /
cd C:\Automation e2e\TeamAyman\Test
>test.txt echo Errors in : %TotalErrors%
>>test.txt echo Total Errors :%NumberErrors% 


	IF %NumberErrors% NEQ 0 (
		cd C:\Automation e2e\TeamAyman\Test\screenshots
		"C:\Program Files\WinRAR\rar.exe" -r a "C:\Automation e2e\TeamAyman\Test\screenshots\screenshots.rar"
		XCOPY  "C:\Automation e2e\TeamAyman\Test\screenshots\screenshots.rar" "C:\Program Files (x86)\Jenkins\workspace\TeamAymanE2EScripts"  /S /I /Q /Y /F
		exit 1
	)
	Pause


SETLOCAL
:CheckError
    set /a count=0
    cd /
    cd windows
    c:
    cd C:\Automation e2e\TeamAyman\Test\screenshots

    IF EXIST images (
        cd /
        cd windows
        c:
        cd C:\Automation e2e\TeamAyman\Test\screenshots\images

        for %%x in (*.png) do set /a count+=1
        set /A NumberErrors=count
    )
    cd /
    cd windows
    c:
    cd C:\Program Files (x86)\Jenkins\workspace\LogitudeTestDevOps\Logitude\AngularModules\AngularModules
goto:eof
