# Phase 2A: Analyze Frontend Bundle Size
# Quick analysis of Angular bundle and dependencies

$repoRoot = "C:\LWC_Prod\log-repo"
$angularPath = "$repoRoot\Logitude\AngularModules\AngularModules"
$reportFile = "$repoRoot\refactor\analysis\frontend-analysis.txt"

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  FRONTEND BUNDLE ANALYSIS" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

New-Item -ItemType Directory -Force -Path "$repoRoot\refactor\analysis" | Out-Null

@"
FRONTEND BUNDLE ANALYSIS
Generated: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")

Quick analysis of Angular application structure and dependencies.

"@ | Out-File $reportFile -Encoding UTF8

Write-Host "[1/3] Analyzing package.json dependencies..." -ForegroundColor Yellow

if (Test-Path "$angularPath\package.json") {
    $packageJson = Get-Content "$angularPath\package.json" | ConvertFrom-Json
    
    $prodDeps = ($packageJson.dependencies.PSObject.Properties | Measure-Object).Count
    $devDeps = ($packageJson.devDependencies.PSObject.Properties | Measure-Object).Count
    
    Write-Host "  Production dependencies: $prodDeps" -ForegroundColor White
    Write-Host "  Development dependencies: $devDeps" -ForegroundColor White
    
    "`nPACKAGE DEPENDENCIES" | Add-Content $reportFile
    "Production dependencies: $prodDeps" | Add-Content $reportFile
    "Development dependencies: $devDeps" | Add-Content $reportFile
    "" | Add-Content $reportFile
    
    # List major dependencies
    "Major dependencies:" | Add-Content $reportFile
    $packageJson.dependencies.PSObject.Properties | ForEach-Object {
        "  $($_.Name): $($_.Value)" | Add-Content $reportFile
    }
} else {
    Write-Host "  package.json not found (node_modules removed)" -ForegroundColor Yellow
}

Write-Host "[2/3] Analyzing module structure..." -ForegroundColor Yellow

$moduleFiles = Get-ChildItem -Path $angularPath -Recurse -Filter "Module_*.ts" -ErrorAction SilentlyContinue
$moduleCount = ($moduleFiles | Measure-Object).Count
Write-Host "  Angular modules: $moduleCount" -ForegroundColor White

"`nANGULAR MODULES" | Add-Content $reportFile
"Total modules: $moduleCount" | Add-Content $reportFile
"" | Add-Content $reportFile
"Modules found:" | Add-Content $reportFile
$moduleFiles | ForEach-Object {
    "  $($_.Name)" | Add-Content $reportFile
}

Write-Host "[3/3] Analyzing component count..." -ForegroundColor Yellow

$components = Get-ChildItem -Path $angularPath -Recurse -Filter "*Component.ts" -ErrorAction SilentlyContinue
$componentCount = ($components | Measure-Object).Count
Write-Host "  Components: $componentCount" -ForegroundColor White

$services = Get-ChildItem -Path $angularPath -Recurse -Filter "*Service.ts" -ErrorAction SilentlyContinue
$serviceCount = ($services | Measure-Object).Count
Write-Host "  Services: $serviceCount" -ForegroundColor White

"`nAPPLICATION STRUCTURE" | Add-Content $reportFile
"Components: $componentCount" | Add-Content $reportFile
"Services: $serviceCount" | Add-Content $reportFile
"" | Add-Content $reportFile

"`n========================================" | Add-Content $reportFile
"OPTIMIZATION OPPORTUNITIES" | Add-Content $reportFile
"========================================" | Add-Content $reportFile
"" | Add-Content $reportFile
"1. LAZY LOADING:" | Add-Content $reportFile
"   - $moduleCount modules could be lazy loaded" | Add-Content $reportFile
"   - Expected impact: 60-70% smaller initial bundle" | Add-Content $reportFile
"" | Add-Content $reportFile
"2. TREE SHAKING:" | Add-Content $reportFile
"   - Review unused dependencies" | Add-Content $reportFile
"   - Remove duplicate UI libraries" | Add-Content $reportFile
"   - Expected impact: 20-30% smaller bundle" | Add-Content $reportFile
"" | Add-Content $reportFile
"3. CODE SPLITTING:" | Add-Content $reportFile
"   - Split vendor bundle from application code" | Add-Content $reportFile
"   - Expected impact: Better caching, faster updates" | Add-Content $reportFile
"" | Add-Content $reportFile
"4. ANGULAR UPGRADE:" | Add-Content $reportFile
"   - Current: Angular 9.x" | Add-Content $reportFile
"   - Target: Angular 17/18" | Add-Content $reportFile
"   - Expected impact: 40-50% faster build + runtime" | Add-Content $reportFile

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  ANALYSIS COMPLETE" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "`nReport saved to:" -ForegroundColor Green
Write-Host "  $reportFile" -ForegroundColor White
Write-Host "========================================`n" -ForegroundColor Cyan

