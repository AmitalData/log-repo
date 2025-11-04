@echo off
REM Clean up output.json files before merging mochawesome reports
REM This prevents output.json from being included in the merge operation

if exist "C:\CypressCloudSmokeTests\Reports\output.json" (
    echo Deleting C:\CypressCloudSmokeTests\Reports\output.json
    del /f /q "C:\CypressCloudSmokeTests\Reports\output.json"
)

if exist "C:\CypressCloudSmokeTests\Reports\Json\output.json" (
    echo Deleting C:\CypressCloudSmokeTests\Reports\Json\output.json
    del /f /q "C:\CypressCloudSmokeTests\Reports\Json\output.json"
)

echo Cleanup complete. output.json files removed.
