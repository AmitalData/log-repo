SETLOCAL enabledelayedexpansion
SET NumberErrors=0
SET TotalErrors

cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\LogitudeTestDevOps\Logitude\AngularModules\AngularModules


FOR /L %%A IN (1,1,1) DO (  
rem call npm run e2e -- --params.Env="test_1209" --params.Team="mohammad" --suite=login,RevGLAccount
rem CALL :CheckError "Create RevGLAccount"

   cmd /c call npm run do-e2e -- --params.Env="test_1209" --params.Team="mohammad" --suite=login,NewChartOfAccount
   CALL :CheckError "Create NewChartOfAccount"

  cmd /c call npm run do-e2e -- --params.Env="test_1209" --params.Team="mohammad" --suite=login,BankAccount
  CALL :CheckError "Create BankAccount"

  cmd /c call npm run do-e2e -- --params.Env="test_1209" --params.Team="mohammad" --suite=login,VendorGLAccount
  CALL :CheckError "Create VendorGLAccount"

  cmd /c call npm run do-e2e -- --params.Env="test_1209" --params.Team="mohammad" --suite=login,CustomerGLAccount
  CALL :CheckError "Create CustomerGLAccount"

 cmd /c call npm run do-e2e -- --params.Env="test_1209" --params.Team="mohammad" --suite=login,ARPayment
 CALL :CheckError "Create ARPayment"

rem call npm run e2e -- --params.Env="test_1071" --params.Team="mohammad" --suite=login,PaymentCheque
rem CALL :CheckError "Create PaymentCheque"
)
cd /
cd C:\Automation e2e\TeamMohammad\Test
>test.txt echo Errors in : %TotalErrors%
>>test.txt echo Total Errors :%NumberErrors% 

IF %NumberErrors% NEQ 0 (
	 cd C:\Automation e2e\TeamMohammad\Test\screenshots
	"C:\Program Files\WinRAR\rar.exe" -r a "C:\Automation e2e\TeamMohammad\Test\screenshots\screenshots.rar"
	XCOPY  "C:\Automation e2e\TeamMohammad\Test\screenshots\screenshots.rar" "C:\Program Files (x86)\Jenkins\workspace\TeamMohammadE2EScripts"  /S /I /Q /Y /F
	exit 1
)


SETLOCAL
:CheckError
    set /a count=0
    cd /
    cd windows
    c:
    cd C:\Automation e2e\TeamMohammad\Test\screenshots

    IF EXIST images (
        cd /
        cd windows
        c:
        cd C:\Automation e2e\TeamMohammad\Test\screenshots\images


        for %%x in (*.png) do set /a count+=1
        set /A NumberErrors=count
    )
    cd /
    cd windows
    c:
    cd C:\Program Files (x86)\Jenkins\workspace\LogitudeTestDevOps\Logitude\AngularModules\AngularModules
goto:eof
