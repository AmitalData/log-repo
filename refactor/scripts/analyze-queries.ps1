# Phase 2A: Analyze for N+1 Query Patterns
# This script searches for common N+1 query anti-patterns

$repoRoot = "C:\LWC_Prod\log-repo"
$reportFile = "$repoRoot\refactor\analysis\n1-queries-analysis.txt"
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  N+1 QUERY PATTERN ANALYSIS" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Create analysis folder
New-Item -ItemType Directory -Force -Path "$repoRoot\refactor\analysis" | Out-Null

# Initialize report
@"
N+1 QUERY PATTERN ANALYSIS
Generated: $timestamp

This report identifies potential N+1 query issues in the codebase.

N+1 queries occur when:
1. You load a collection of entities
2. Then loop through them
3. And access a navigation property inside the loop
4. Causing N additional queries (one per entity)

Example BAD pattern:
  var shipments = context.Shipments.ToList();  // 1 query
  foreach(var s in shipments) {
      var customer = s.Customer.Name;  // N queries!
  }

Example GOOD pattern:
  var shipments = context.Shipments
      .Include(s => s.Customer)  // Eager load
      .ToList();  // 1 query total

========================================

"@ | Out-File $reportFile -Encoding UTF8

Write-Host "[1/5] Searching for foreach loops with navigation property access..." -ForegroundColor Yellow

# Pattern 1: foreach with navigation property access
$pattern1Files = Get-ChildItem -Path "$repoRoot\Logitude" -Recurse -Include *.cs -ErrorAction SilentlyContinue |
    Select-String -Pattern "foreach\s*\(" -Context 0,10 |
    Where-Object { $_.Context.PostContext -match "\.\w+\." } |
    Select-Object -First 20

$count1 = ($pattern1Files | Measure-Object).Count
Write-Host "  Found: $count1 potential cases" -ForegroundColor $(if($count1 -gt 0){"Yellow"}else{"Green"})

"`nPATTERN 1: foreach loops with property access" | Add-Content $reportFile
"Found $count1 potential cases:" | Add-Content $reportFile
"" | Add-Content $reportFile

$pattern1Files | ForEach-Object {
    "File: $($_.Path)" | Add-Content $reportFile
    "Line $($_.LineNumber): $($_.Line.Trim())" | Add-Content $reportFile
    "" | Add-Content $reportFile
}

Write-Host "[2/5] Searching for ToList() followed by loops..." -ForegroundColor Yellow

# Pattern 2: ToList() immediately followed by foreach
$pattern2Files = Get-ChildItem -Path "$repoRoot\Logitude" -Recurse -Include *.cs -ErrorAction SilentlyContinue |
    Select-String -Pattern "\.ToList\(\)" -Context 0,3 |
    Where-Object { $_.Context.PostContext -match "foreach" } |
    Select-Object -First 20

$count2 = ($pattern2Files | Measure-Object).Count
Write-Host "  Found: $count2 potential cases" -ForegroundColor $(if($count2 -gt 0){"Yellow"}else{"Green"})

"`nPATTERN 2: ToList() followed by foreach" | Add-Content $reportFile
"Found $count2 potential cases:" | Add-Content $reportFile
"" | Add-Content $reportFile

$pattern2Files | ForEach-Object {
    "File: $($_.Path)" | Add-Content $reportFile
    "Line $($_.LineNumber): $($_.Line.Trim())" | Add-Content $reportFile
    "" | Add-Content $reportFile
}

Write-Host "[3/5] Searching for queries inside loops..." -ForegroundColor Yellow

# Pattern 3: Repository/context calls inside loops
$pattern3Files = Get-ChildItem -Path "$repoRoot\Logitude" -Recurse -Include *.cs -ErrorAction SilentlyContinue |
    Select-String -Pattern "foreach\s*\(" -Context 0,15 |
    Where-Object { $_.Context.PostContext -match "(Repository|GetById|FirstOrDefault|Where)" } |
    Select-Object -First 20

$count3 = ($pattern3Files | Measure-Object).Count
Write-Host "  Found: $count3 potential cases" -ForegroundColor $(if($count3 -gt 0){"Yellow"}else{"Green"})

"`nPATTERN 3: Database queries inside loops" | Add-Content $reportFile
"Found $count3 potential cases:" | Add-Content $reportFile
"" | Add-Content $reportFile

$pattern3Files | ForEach-Object {
    "File: $($_.Path)" | Add-Content $reportFile
    "Line $($_.LineNumber): $($_.Line.Trim())" | Add-Content $reportFile
    "" | Add-Content $reportFile
}

Write-Host "[4/5] Searching for missing Include() statements..." -ForegroundColor Yellow

# Pattern 4: Queries without Include but with ToList
$pattern4Files = Get-ChildItem -Path "$repoRoot\Logitude\Simplog.Data" -Recurse -Include *Repository.cs -ErrorAction SilentlyContinue |
    Select-String -Pattern "\.ToList\(\)|\.FirstOrDefault\(\)" |
    Where-Object { $_.Line -notmatch "Include" } |
    Select-Object -First 30

$count4 = ($pattern4Files | Measure-Object).Count
Write-Host "  Found: $count4 potential cases" -ForegroundColor $(if($count4 -gt 0){"Yellow"}else{"Green"})

"`nPATTERN 4: Queries without eager loading" | Add-Content $reportFile
"Found $count4 potential cases in repositories:" | Add-Content $reportFile
"" | Add-Content $reportFile

$pattern4Files | ForEach-Object {
    "File: $($_.Path)" | Add-Content $reportFile
    "Line $($_.LineNumber): $($_.Line.Trim())" | Add-Content $reportFile
    "" | Add-Content $reportFile
}

Write-Host "[5/5] Searching for Select with navigation properties..." -ForegroundColor Yellow

# Pattern 5: Select statements with navigation properties
$pattern5Files = Get-ChildItem -Path "$repoRoot\Logitude" -Recurse -Include *.cs -ErrorAction SilentlyContinue |
    Select-String -Pattern "\.Select\(.*=>\s*.*\.\w+\.\w+" |
    Select-Object -First 20

$count5 = ($pattern5Files | Measure-Object).Count
Write-Host "  Found: $count5 potential cases" -ForegroundColor $(if($count5 -gt 0){"Yellow"}else{"Green"})

"`nPATTERN 5: Select with navigation properties" | Add-Content $reportFile
"Found $count5 potential cases:" | Add-Content $reportFile
"" | Add-Content $reportFile

$pattern5Files | ForEach-Object {
    "File: $($_.Path)" | Add-Content $reportFile
    "Line $($_.LineNumber): $($_.Line.Trim())" | Add-Content $reportFile
    "" | Add-Content $reportFile
}

# Summary
$totalIssues = $count1 + $count2 + $count3 + $count4 + $count5

"`n========================================" | Add-Content $reportFile
"SUMMARY" | Add-Content $reportFile
"========================================" | Add-Content $reportFile
"" | Add-Content $reportFile
"Total potential N+1 issues found: $totalIssues" | Add-Content $reportFile
"" | Add-Content $reportFile
"Pattern 1 (foreach with navigation): $count1" | Add-Content $reportFile
"Pattern 2 (ToList + foreach): $count2" | Add-Content $reportFile
"Pattern 3 (queries in loops): $count3" | Add-Content $reportFile
"Pattern 4 (missing Include): $count4" | Add-Content $reportFile
"Pattern 5 (Select navigation): $count5" | Add-Content $reportFile
"" | Add-Content $reportFile
"RECOMMENDATION:" | Add-Content $reportFile
"These are potential issues that require manual review." | Add-Content $reportFile
"Not all findings are actual problems - context matters." | Add-Content $reportFile
"Priority: Review top 10-20 most frequently executed code paths." | Add-Content $reportFile
"" | Add-Content $reportFile
"Expected impact if fixed: 30-50% faster API responses" | Add-Content $reportFile

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  ANALYSIS COMPLETE" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "`nTotal potential issues: $totalIssues" -ForegroundColor Yellow
Write-Host "`nReport saved to:" -ForegroundColor Green
Write-Host "  $reportFile" -ForegroundColor White
Write-Host "`nNote: These are potential issues requiring manual review." -ForegroundColor Yellow
Write-Host "========================================`n" -ForegroundColor Cyan

