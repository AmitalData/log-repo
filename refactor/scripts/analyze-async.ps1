# Phase 2A: Analyze for Async/Await Opportunities
# Identifies synchronous database calls that should be async

$repoRoot = "C:\LWC_Prod\log-repo"
$reportFile = "$repoRoot\refactor\analysis\async-opportunities.txt"
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  ASYNC/AWAIT OPPORTUNITY ANALYSIS" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

New-Item -ItemType Directory -Force -Path "$repoRoot\refactor\analysis" | Out-Null

@"
ASYNC/AWAIT OPPORTUNITY ANALYSIS
Generated: $timestamp

This report identifies synchronous database calls that should be async.

Why async matters:
- Synchronous calls block threads
- Async allows threads to handle other requests
- Can increase throughput by 2-3x

Example BAD pattern:
  public ShipmentPM Get(string id) {
      return repo.GetById(id);  // Blocks thread!
  }

Example GOOD pattern:
  public async Task<ShipmentPM> GetAsync(string id) {
      return await repo.GetByIdAsync(id);  // Non-blocking!
  }

========================================

"@ | Out-File $reportFile -Encoding UTF8

Write-Host "[1/4] Analyzing Controllers..." -ForegroundColor Yellow

# Find controller actions without async
$syncControllers = Get-ChildItem -Path "$repoRoot\Logitude\JustWebFreight\WebFreight.Web\Controllers" -Recurse -Include *.cs -ErrorAction SilentlyContinue |
    Select-String -Pattern "public\s+(IHttpActionResult|ActionResult)\s+\w+" |
    Where-Object { $_.Line -notmatch "async" } |
    Select-Object -First 50

$countControllers = ($syncControllers | Measure-Object).Count
Write-Host "  Found: $countControllers synchronous controller actions" -ForegroundColor $(if($countControllers -gt 100){"Red"}elseif($countControllers -gt 50){"Yellow"}else{"Green"})

"`nCONTROLLER ACTIONS WITHOUT ASYNC" | Add-Content $reportFile
"Found $countControllers synchronous actions (showing first 50):" | Add-Content $reportFile
"" | Add-Content $reportFile

$syncControllers | ForEach-Object {
    "File: $($_.Path.Replace($repoRoot, '.'))" | Add-Content $reportFile
    "Line $($_.LineNumber): $($_.Line.Trim())" | Add-Content $reportFile
    "" | Add-Content $reportFile
}

Write-Host "[2/4] Analyzing Repositories..." -ForegroundColor Yellow

# Find repository methods without async
$syncRepos = Get-ChildItem -Path "$repoRoot\Logitude\Simplog.Data" -Recurse -Include *Repository.cs -ErrorAction SilentlyContinue |
    Select-String -Pattern "public\s+\w+\s+\w+\(" |
    Where-Object { $_.Line -notmatch "async|Task" -and $_.Line -match "(GetById|GetBy|Save|Insert|Update|Delete)" } |
    Select-Object -First 50

$countRepos = ($syncRepos | Measure-Object).Count
Write-Host "  Found: $countRepos synchronous repository methods" -ForegroundColor $(if($countRepos -gt 100){"Red"}elseif($countRepos -gt 50){"Yellow"}else{"Green"})

"`nREPOSITORY METHODS WITHOUT ASYNC" | Add-Content $reportFile
"Found $countRepos synchronous methods (showing first 50):" | Add-Content $reportFile
"" | Add-Content $reportFile

$syncRepos | ForEach-Object {
    "File: $($_.Path.Replace($repoRoot, '.'))" | Add-Content $reportFile
    "Line $($_.LineNumber): $($_.Line.Trim())" | Add-Content $reportFile
    "" | Add-Content $reportFile
}

Write-Host "[3/4] Analyzing Business Logic..." -ForegroundColor Yellow

# Find BL methods with database calls but no async
$syncBL = Get-ChildItem -Path "$repoRoot\Logitude\Logitude.BL" -Recurse -Include *.cs -ErrorAction SilentlyContinue |
    Select-String -Pattern "Repository|SaveChanges|ToList\(\)" -Context 10,0 |
    Where-Object { $_.Context.PreContext -notmatch "async|Task" } |
    Select-Object -First 30

$countBL = ($syncBL | Measure-Object).Count
Write-Host "  Found: $countBL potential synchronous BL methods" -ForegroundColor $(if($countBL -gt 50){"Red"}elseif($countBL -gt 20){"Yellow"}else{"Green"})

"`nBUSINESS LOGIC WITH SYNC DATABASE CALLS" | Add-Content $reportFile
"Found $countBL potential cases (showing first 30):" | Add-Content $reportFile
"" | Add-Content $reportFile

$syncBL | ForEach-Object {
    "File: $($_.Path.Replace($repoRoot, '.'))" | Add-Content $reportFile
    "Line $($_.LineNumber): $($_.Line.Trim())" | Add-Content $reportFile
    "" | Add-Content $reportFile
}

Write-Host "[4/4] Checking for .Result and .Wait() anti-patterns..." -ForegroundColor Yellow

# Find .Result or .Wait() which blocks async code
$blockingCalls = Get-ChildItem -Path "$repoRoot\Logitude" -Recurse -Include *.cs -ErrorAction SilentlyContinue |
    Select-String -Pattern "\.Result(?!;)|\.Wait\(\)" |
    Select-Object -First 20

$countBlocking = ($blockingCalls | Measure-Object).Count
Write-Host "  Found: $countBlocking blocking async calls" -ForegroundColor $(if($countBlocking -gt 0){"Red"}else{"Green"})

"`nBLOCKING ASYNC CALLS (.Result / .Wait())" | Add-Content $reportFile
"Found $countBlocking blocking patterns:" | Add-Content $reportFile
"WARNING: These defeat the purpose of async!" | Add-Content $reportFile
"" | Add-Content $reportFile

$blockingCalls | ForEach-Object {
    "File: $($_.Path.Replace($repoRoot, '.'))" | Add-Content $reportFile
    "Line $($_.LineNumber): $($_.Line.Trim())" | Add-Content $reportFile
    "" | Add-Content $reportFile
}

# Summary
$totalIssues = $countControllers + $countRepos + $countBL + $countBlocking

"`n========================================" | Add-Content $reportFile
"SUMMARY" | Add-Content $reportFile
"========================================" | Add-Content $reportFile
"" | Add-Content $reportFile
"Total async opportunities: $totalIssues" | Add-Content $reportFile
"" | Add-Content $reportFile
"Synchronous controllers: $countControllers" | Add-Content $reportFile
"Synchronous repositories: $countRepos" | Add-Content $reportFile
"Synchronous BL methods: $countBL" | Add-Content $reportFile
"Blocking async calls: $countBlocking" | Add-Content $reportFile
"" | Add-Content $reportFile
"RECOMMENDATION:" | Add-Content $reportFile
"1. Start with controllers - convert to async Task<IHttpActionResult>" | Add-Content $reportFile
"2. Then repositories - add Async suffix and return Task<T>" | Add-Content $reportFile
"3. Then business logic - propagate async through call chain" | Add-Content $reportFile
"4. Fix any .Result or .Wait() calls immediately" | Add-Content $reportFile
"" | Add-Content $reportFile
"Expected impact: 2-3x more concurrent request capacity" | Add-Content $reportFile
"Effort: Medium (requires careful testing)" | Add-Content $reportFile

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  ANALYSIS COMPLETE" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "`nTotal opportunities: $totalIssues" -ForegroundColor Yellow
Write-Host "`nReport saved to:" -ForegroundColor Green
Write-Host "  $reportFile" -ForegroundColor White
Write-Host "========================================`n" -ForegroundColor Cyan

