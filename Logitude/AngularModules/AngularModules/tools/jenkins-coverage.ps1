<#
.SYNOPSIS
    Jenkins PowerShell: run Jest, parse coverage, compare to threshold, optionally send alert.
.DESCRIPTION
    - Fetches PR author email only when building a PR (prid > 0); otherwise uses devops only.
    - Runs Jest via jest-runner.js; reads coverage from test-results/coverage-summary.json
      (written by jest-runner) or falls back to parsing jest-output.log.
    - Store secrets in Jenkins credentials / env: AZURE_PAT, SENDGRID_API_KEY (not in script).
#>

param(
    [string]$AngularRoot = "E:\Jenkins\LogBoxTestDevOps\Logitude\AngularModules\AngularModules",
    [string]$VariableGroupUrl = "https://amitaldata.visualstudio.com/Amital/_apis/distributedtask/variablegroups/10?api-version=5.1-preview.1",
    [string]$RepoPrUrlTemplate = "https://dev.azure.com/amitaldata/Amital/_apis/git/repositories/log-repo/pullRequests/{0}?api-version=7.1-preview.1"
)

$ErrorActionPreference = "Stop"

# --- Secrets: use Jenkins env vars or credentials ---
$connectionToken = $env:AZURE_PAT
$SendGridApiKey = $env:SENDGRID_API_KEY
if (-not $connectionToken) { Write-Error "AZURE_PAT (Azure DevOps PAT) not set. Configure in Jenkins." }
if (-not $SendGridApiKey) { Write-Error "SENDGRID_API_KEY not set. Configure in Jenkins." }

$base64AuthInfo = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes(":$connectionToken"))

# --- Threshold from Azure variable group ---
$threshhold = (Invoke-RestMethod -Uri $VariableGroupUrl -Headers @{ authorization = "Basic $base64AuthInfo" } -Method Get -ContentType "application/json").variables.Amital_Main_Frontend.value

# --- PR author email: only when this is a PR build (prid set and > 0) ---
$emailTo = @("devops@amital.co.il")
$psprid = $env:prid
if ($psprid -match '^\d+$' -and [int]$psprid -gt 0) {
    try {
        $prUrl = $RepoPrUrlTemplate -f $psprid
        $prDetails = Invoke-RestMethod -Uri $prUrl -Headers @{ Authorization = "Basic $base64AuthInfo" } -Method Get
        $emailTo = @($prDetails.createdBy.uniqueName, "devops@amital.co.il")
    } catch {
        Write-Warning "Could not fetch PR $psprid (not a PR build or API error). Using devops only for alerts."
    }
}

function Send-CoverageAlert {
    param([double]$Total, [double]$Threshold)
    $body = @{
        personalizations = @(
            @{
                to = ($emailTo | ForEach-Object { @{ email = $_ } })
                subject = "Amital_Main Frontend Coverage Dropped: $Total%"
            }
        )
        from = @{ email = "devops@amital.co.il" }
        content = @(
            @{
                type  = "text/plain"
                value = @"
Frontend unit test coverage dropped below threshold.

Total Coverage : $Total%
Threshold      : $Threshold%

Build machine  : $env:COMPUTERNAME
Jenkins job    : $env:JOB_NAME
Build number   : $env:BUILD_NUMBER

Please review the Jest results.
"@
            }
        )
    } | ConvertTo-Json -Depth 5

    Invoke-RestMethod -Uri "https://api.sendgrid.com/v3/mail/send" -Method POST `
        -Headers @{ Authorization = "Bearer $SendGridApiKey"; "Content-Type" = "application/json" } -Body $body
}

# --- Ensure clean install (fixes __ngcc_entry_points__.json); optional: uncomment to run every time ---
# Set-Location $AngularRoot; npm run ci-install

Set-Location $AngularRoot
Remove-Item jest-output.log -ErrorAction SilentlyContinue
pwsh -Command "node tools\jest-runner.js --maxWorkers 8 --maxOldSpaceMB 65536 --mode parallelAll --detailed --enableJUnit 2>&1 | Tee-Object -FilePath jest-output.log"

$coverageJsonPath = Join-Path $AngularRoot "test-results\coverage-summary.json"
$coverage = $null

if (Test-Path $coverageJsonPath) {
    $coverage = Get-Content $coverageJsonPath -Raw | ConvertFrom-Json
}

if (-not $coverage) {
    $logPath = Join-Path $AngularRoot "jest-output.log"
    if (-not (Test-Path $logPath)) {
        Write-Error "Jest did not produce output. Ensure 'npm install' (or 'npm run ci-install') runs before this step, then run Jest again."
    }
    $content = Get-Content $logPath -Raw
    if ($content -match 'ALL\s+([\d.]+)%\s+([\d.]+)%\s+([\d.]+)%\s+([\d.]+)%\s+([\d.]+)%\s+([\d.]+)%') {
        $coverage = [PSCustomObject]@{
            Statements = [double]$matches[1]
            Branches   = [double]$matches[2]
            Functions  = [double]$matches[3]
            Lines      = [double]$matches[4]
            Average    = [double]$matches[5]
            Total     = [double]$matches[6]
        }
    }
}

if (-not $coverage) {
    Write-Error "Coverage summary not found. Jest may have failed before writing results (e.g. missing node_modules). Run 'npm run ci-install' then re-run this job."
}

$total = [double]$coverage.Total
Write-Host "Coverage results:"
$coverage.PSObject.Properties | ForEach-Object { Write-Host "$($_.Name): $($_.Value)%" }

$updatethresholdbody = "{`"id`":10,`"type`":`"Vsts`",`"name`":`"Unit Testing Thresholds`",`"variables`":{`"Amital_Main_Frontend`":{`"isSecret`":false,`"value`":`"$total`"}}}"

if ($total -gt $threshhold) {
    Write-Host "Coverage $total% is above threshold $threshhold% - updating variable group."
    Invoke-RestMethod -Uri $VariableGroupUrl -Headers @{ authorization = "Basic $base64AuthInfo" } -Method Put -Body $updatethresholdbody -ContentType "application/json"
}
if ($total -lt $threshhold) {
    Write-Host "Coverage $total% is below threshold $threshhold% - sending alert and failing build."
    Send-CoverageAlert -Total $total -Threshold $threshhold
    exit 1
}
