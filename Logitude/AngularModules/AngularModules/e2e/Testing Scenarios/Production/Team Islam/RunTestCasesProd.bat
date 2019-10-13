
SETLOCAL enabledelayedexpansion
SET err=0
SET TotalErrors

cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\2019.R3.DevOps\Logitude\AngularModules\AngularModules

set NumberErrors=0

FOR /L %%A IN (1,1,1) DO (

--Report--
 cmd /c call  npm run e2e -- --params.Env="prod_1" --params.Team="islam" --suite=login,Reports>D:\E2ETeamIslamReport\Report.log
 CALL :CheckError "Run Report"
 
 --DocOut--
cmd /c call  npm run e2e -- --params.Env="prod_1" --params.ShipParams.ShipmentLevelCode="D" --params.ShipParams.Direction="Export" --params.ShipParams.TransportMode="A" --params.ShipParams.ShipmentType="" --params.ShipParams.ShipmentEditTabs="docs" --params.Team="islam" --suite=login,DocOut>D:\E2ETeamIslamReport\Report.log
  CALL :CheckError "Print Document"
  
  
--ShipmentView-- 
  cmd /c call  npm run e2e -- --params.Env="prod_1" --params.Team="islam" --suite=login,ShipmentView>D:\E2ETeamIslamReport\Report.log
 CALL :CheckError "ShipmentView"
   
--CompanyAddressSetting
cmd /c call npm run e2e -- --params.Env="prod_1" --params.Team="islam" --suite=login,CompanyAddressSetting>D:\E2ETeamIslamReport\Report.log
  CALL :CheckError "CompanyAddressSetting"
  
--NewAgent
  cmd /c call npm run e2e -- --params.Env="prod_1" --params.Team="islam" --suite=login,NewAgent>D:\E2ETeamIslamReport\Report.log
 CALL :CheckError "NewAgent"
   
--NewUser--
 cmd /c call npm run e2e -- --params.Env="prod_1" --params.Team="islam" --suite=login,NewUser>D:\E2ETeamIslamReport\Report.log
 CALL :CheckError "NewUser"
 
--NewShipper--
rem cmd /c call npm run e2e -- --params.Env="prod_1" --params.Team="islam" --suite=login,NewShipper>D:\E2ETeamIslamReport\Report.log
rem CALL :CheckError "NewShipper"
 

)




cd /
cd C:\Automation e2e\TeamIslam\Prod

>test.txt echo Errors in : %TotalErrors%
>>test.txt echo Total Errors :%NumberErrors% 


IF %NumberErrors% NEQ 0 ( 
  exit 1
)

pause


SETLOCAL
:CheckError 
for /f %%a in ('type D:\E2ETeamIslamReport\Report.log ^| find /c /i "error"') DO (if %%a NEQ 0 set TotalErrors=%TotalErrors%    %~1%NL% & @SET /a "NumberErrors=%NumberErrors%+1" )

for /f %%a in ('type D:\E2ETeamIslamReport\Report.log ^| find /c /i "This will be an error in future versions"') DO (if %%a NEQ 0 set TotalErrors=%TotalErrors%    %~1%NL% & @SET /a "NumberErrors=%NumberErrors%-1" )

goto:eof
