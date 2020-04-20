SETLOCAL enabledelayedexpansion
SET NumberErrors=0
cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\2019.R3.DevOps\Logitude\AngularModules\AngularModules



FOR /L %%A IN (1,1,1) DO (


 cmd /c call  npm run e2e -- --params.Env="Prod_Staging" --params.Team="islamProd" --suite=login,Reports
 CALL :CheckError "Run Report"
 
cmd /c call  npm run e2e -- --params.Env="Prod_Staging" --params.ShipParams.ShipmentLevelCode="D" --params.ShipParams.Direction="Export" --params.ShipParams.TransportMode="A" --params.ShipParams.ShipmentType="" --params.ShipParams.ShipmentEditTabs="docs" --params.Team="islamProd" --suite=login,DocOut
  CALL :CheckError "Print Document"
  
  
--ShipmentView-- 
  cmd /c call  npm run e2e -- --params.Env="Prod_Staging" --params.Team="islamProd" --suite=login,ShipmentView
CALL :CheckError "ShipmentView"
   
--CompanyAddressSetting
 cmd /c call npm run e2e -- --params.Env="Prod_Staging" --params.Team="islamProd" --suite=login,CompanyAddressSetting
  CALL :CheckError "CompanyAddressSetting"
  
--NewAgent
  cmd /c call npm run e2e -- --params.Env="Prod_Staging" --params.Team="islamProd" --suite=login,NewAgent
 CALL :CheckError "NewAgent"
   
--NewUser--
 cmd /c call npm run e2e -- --params.Env="Prod_Staging" --params.Team="islamProd" --suite=login,NewUser
 CALL :CheckError "NewUser"
 
--NewShipper--
rem cmd /c call npm run e2e -- --params.Env="Prod_Staging" --params.Team="islam" --suite=login,NewShipper>D:\E2ETeamIslamReport\Report.log
rem  CALL :CheckError "NewShipper"
 

)

cd /
cd C:\Automation e2e\TeamIslam\Prod
>test.txt echo Total Errors :%NumberErrors% 

IF %NumberErrors% NEQ 0 ( 
  exit 1
)


SETLOCAL
:CheckError
    set /a count=0
    cd /
    cd windows
    c:
    cd C:\Automation e2e\TeamIslam\prod\screenshots

    IF EXIST images (
        cd /
        cd windows
        c:
        cd C:\Automation e2e\TeamIslam\Prod\screenshots\images

        for %%x in (*.png) do set /a count+=1
        set /A NumberErrors=count
    )
    cd /
    cd windows
    c:
    cd C:\Program Files (x86)\Jenkins\workspace\2019.R3.DevOps\Logitude\AngularModules\AngularModules

goto:eof
