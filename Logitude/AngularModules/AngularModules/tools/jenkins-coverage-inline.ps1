# Jenkins PowerShell - Frontend coverage: run Jest, parse coverage, compare threshold, send alert if below.
# Set in Jenkins (Credentials Binding): AZURE_PAT, SENDGRID_API_KEY
# Or set below: $connectionToken = "your-azure-pat"; $SendGridApiKey = "your-sendgrid-key"

$connectionToken = $env:AZURE_PAT
$SendGridApiKey = $env:SENDGRID_API_KEY
if (-not $connectionToken) { $connectionToken = "PASTE_AZURE_PAT_OR_SET_AZURE_PAT_IN_JENKINS" }
if (-not $SendGridApiKey) { $SendGridApiKey = "PASTE_SENDGRID_KEY_OR_SET_SENDGRID_API_KEY_IN_JENKINS" }

$base64AuthInfo = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes(":$($connectionToken)"))
$organization = "amitaldata"
$project = "Amital"
$URL = "https://amitaldata.visualstudio.com/Amital/_apis/distributedtask/variablegroups/10?api-version=5.1-preview.1"
$threshhold = (Invoke-RestMethod -Uri $URL -Headers @{ authorization = "Basic $base64AuthInfo" } -Method Get -ContentType "application/json").variables.Amital_Main_Frontend.value

# PR author email: only when this is a PR build (prid set and > 0)
$email = "devops@amital.co.il"
$psprid = $env:prid
if ($psprid -match '^\d+$' -and [int]$psprid -gt 0) {
    try {
        $prUrl = "https://dev.azure.com/$organization/$project/_apis/git/repositories/log-repo/pullRequests/$psprid`?api-version=7.1-preview.1"
        Write-Host $prUrl
        Write-Host $psprid
        $prDetails = Invoke-RestMethod -Uri $prUrl -Headers @{ Authorization = "Basic $base64AuthInfo" } -Method Get
        $email = $prDetails.createdBy.uniqueName
    } catch {
        Write-Warning "Could not fetch PR $psprid (not a PR build or API error). Using devops only."
    }
}

function Send-CoverageAlert {
    param (
        [double]$Total,
        [double]$Threshold
    )
    $body = @{
        personalizations = @(
            @{
                to = @(
                    @{ email = $email },
                    @{ email = "devops@amital.co.il" }
                )
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

    Invoke-RestMethod `
        -Uri "https://api.sendgrid.com/v3/mail/send" `
        -Method POST `
        -Headers @{
            Authorization = "Bearer $SendGridApiKey"
            "Content-Type" = "application/json"
        } `
        -Body $body
}

$angularRoot = "E:\Jenkins\LogBoxTestDevOps\Logitude\AngularModules\AngularModules"
Set-Location $angularRoot

# Ensure node_modules exists (fixes __ngcc_entry_points__.json and missing Jest binary)
Write-Host "Running ci-install (remove node_modules + npm install)..."
npm run ci-install
if ($LASTEXITCODE -ne 0) {
    Write-Error "ci-install failed. Fix npm/network issues and re-run."
    exit 1
}

pwsh -Command "Remove-Item jest-output.log -ErrorAction SilentlyContinue; node tools\jest-runner.js --maxWorkers 8 --maxOldSpaceMB 65536 --mode parallelAll --detailed --enableJUnit 2>&1 | Tee-Object -FilePath jest-output.log"

$coverageJsonPath = Join-Path $angularRoot "test-results\coverage-summary.json"
$coverageFile = Join-Path $angularRoot "jest-output.log"
$coverage = $null

# Prefer machine-readable summary from jest-runner; fall back to parsing log
if (Test-Path $coverageJsonPath) {
    $j = Get-Content $coverageJsonPath -Raw | ConvertFrom-Json
    $coverage = @{
        Statements = [double]$j.Statements
        Branches   = [double]$j.Branches
        Functions  = [double]$j.Functions
        Lines      = [double]$j.Lines
        Average    = [double]$j.Average
        Total      = [double]$j.Total
    }
}

if (-not $coverage -and (Test-Path $coverageFile)) {
    $content = Get-Content $coverageFile -Raw
    if ($content -match 'ALL\s+([\d.]+)%\s+([\d.]+)%\s+([\d.]+)%\s+([\d.]+)%\s+([\d.]+)%\s+([\d.]+)%') {
        $coverage = @{
            Statements = [double]$matches[1]
            Branches   = [double]$matches[2]
            Functions  = [double]$matches[3]
            Lines      = [double]$matches[4]
            Average    = [double]$matches[5]
            Total      = [double]$matches[6]
        }
    }
}

if (-not $coverage) {
    Write-Error "Coverage summary not found. Jest may have failed before writing results (e.g. missing node_modules). Run 'npm run ci-install' then re-run this job."
    exit 1
}

$total = $coverage['Total']
$updatethresholdbody = "{`"id`":10,`"type`":`"Vsts`",`"name`":`"Unit Testing Thresholds`",`"variables`":{`"Amital_Main_Frontend`":{`"isSecret`":false,`"value`":`"$total`"}}}"

Write-Host "Coverage results:"
$coverage.GetEnumerator() | ForEach-Object {
    Write-Host "$($_.Key): $($_.Value)%"
}

if ($total -gt $threshhold) {
    Write-Host "Coverage $total% is above threshold $threshhold% - updating variable group."
    Invoke-RestMethod -Uri $URL -Headers @{ authorization = "Basic $base64AuthInfo" } -Method Put -Body $updatethresholdbody -ContentType "application/json"
}
if ($total -lt $threshhold) {
    Write-Host "Lower - Please add new code to unit testing"
    Send-CoverageAlert -Total $total -Threshold $threshhold
    exit 1
}
