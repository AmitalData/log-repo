SETLOCAL enabledelayedexpansion
SET NumberErrors=0
SET TotalErrors

cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\LogitudeTestDevOps\Logitude\AngularModules\AngularModules



FOR /L %%A IN (1,1,1) DO (


   
--Report--
  cmd /c call  npm run e2e -- --params.Env="testEnvStaging" --params.Team="islam" --suite=login,Reports>D:\E2ETeamIslamReport\Report.log
  CALL :CheckError "Run Report"

--DocOut--
  cmd /c call  npm run e2e -- --params.Env="testEnvStaging" --params.ShipParams.ShipmentLevelCode="D" --params.ShipParams.Direction="Export" --params.ShipParams.TransportMode="A" --params.ShipParams.ShipmentType="" --params.ShipParams.ShipmentEditTabs="docs" --params.Team="islam" --suite=login,DocOut>D:\E2ETeamIslamReport\Report.log
   CALL :CheckError "Print Document"

--ShipmentView-- 
 cmd /c call  npm run e2e -- --params.Env="testEnvStaging" --params.Team="islam" --suite=login,ShipmentView>D:\E2ETeamIslamReport\Report.log
CALL :CheckError "ShipmentView"
   
--CompanyAddressSetting
   cmd /c call npm run e2e -- --params.Env="testEnvStaging" --params.Team="islam" --suite=login,CompanyAddressSetting>D:\E2ETeamIslamReport\Report.log
   CALL :CheckError "CompanyAddressSetting"
  
--NewAgent
   cmd /c call npm run e2e -- --params.Env="testEnvStaging" --params.Team="islam" --suite=login,NewAgent>D:\E2ETeamIslamReport\Report.log
   CALL :CheckError "NewAgent"
   
--NewUser--
  cmd /c call npm run e2e -- --params.Env="testEnvStaging" --params.Team="islam" --suite=login,NewUser>D:\E2ETeamIslamReport\Report.log
  CALL :CheckError "NewUser"
 
--NewShipper--
  cmd /c call npm run e2e -- --params.Env="testEnvStaging" --params.Team="islam" --suite=login,NewShipper>D:\E2ETeamIslamReport\Report.log
  CALL :CheckError "NewShipper"

 
 
)

cd /
cd C:\Automation e2e\TeamIslam\Test
>test.txt echo Errors in : %TotalErrors%
>>test.txt echo Total Errors :%NumberErrors% 



IF %NumberErrors% NEQ 0 (
 cd C:\Automation e2e\TeamIslam\Test\screenshots
  "C:\Program Files\WinRAR\rar.exe" -r a "C:\Automation e2e\TeamIslam\Test\screenshots.rar"
  XCOPY  "C:\Automation e2e\TeamIslam\Test\screenshots\screenshots.rar" "C:\Program Files (x86)\Jenkins\workspace\TeamIslamE2EScripts"  /S /I /Q /Y /F
  exit 1
)
Pause

SETLOCAL
:CheckError

set /a count=0

cd /
cd windows
c:
cd C:\Automation e2e\TeamIslam\Test\screenshots

IF EXIST images (

cd /
cd windows
c:
cd C:\Automation e2e\TeamIslam\Test\screenshots\images

for %%x in (*.png) do set /a count+=1
set /A NumberErrors=count


)
cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\LogitudeTestDevOps\Logitude\AngularModules\AngularModules

goto:eof
