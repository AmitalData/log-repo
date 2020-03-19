SETLOCAL enabledelayedexpansion
SET NumberErrors=0
SET TotalErrors

cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\Amital.DevOps\Logitude\AngularModules\AngularModules



FOR /L %%A IN (1,1,1) DO (  

call npm run e2e -- --params.Env="cloud" --params.Team="mohammadcloud" --suite=login,BankAccount>D:\TeamMohammadE2E\Cloud\prot.log 2>&1
CALL :CheckError "Create BankAccount"



call npm run e2e -- --params.Env="cloud" --params.Team="mohammadcloud" --suite=login,NewChartOfAccount>D:\TeamMohammadE2E\Cloud\prot.log 2>&1
CALL :CheckError "Create NewChartOfAccount"

call npm run e2e -- --params.Env="cloud" --params.Team="mohammadcloud" --suite=login,VendorGLAccount>D:\TeamMohammadE2E\Cloud\prot.log 2>&1
CALL :CheckError "Create VendorGLAccount"

call npm run e2e -- --params.Env="cloud" --params.Team="mohammadcloud" --suite=login,CustomerGLAccount>D:\TeamMohammadE2E\Cloud\prot.log 2>&1
CALL :CheckError "Create CustomerGLAccount"

call npm run e2e -- --params.Env="cloud" --params.Team="mohammadcloud" --suite=login,ARPayment>D:\TeamMohammadE2E\Cloud\prot.log 2>&1
CALL :CheckError "Create ARPayment"



rem call npm run e2e -- --params.Env="cloud" --params.Team="mohammadcloud" --suite=login,RevGLAccount>D:\TeamMohammadE2E\Cloud\prot.log 2>&1
rem CALL :CheckError "Create RevGLAccount"

rem call npm run e2e -- --params.Env="cloud" --params.Team="mohammadcloud" --suite=login,PaymentCheque>D:\TeamMohammadE2E\Cloud\prot.log 2>&1
rem CALL :CheckError "Create PaymentCheque"


)
cd /
cd C:\Automation e2e\TeamMohammad\Cloud
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
cd C:\Automation e2e\TeamMohammad\Cloud\screenshots

IF EXIST images (

cd /
cd windows
c:
cd C:\Automation e2e\TeamMohammad\Cloud\screenshots\images

for %%x in (*.png) do set /a count+=1
set /A NumberErrors=count


)
cd /
cd windows
c:
cd C:\Program Files (x86)\Jenkins\workspace\Amital.DevOps\Logitude\AngularModules\AngularModules

goto:eof
