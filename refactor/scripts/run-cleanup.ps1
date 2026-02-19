# Simple cleanup script without special characters
param(
    [switch]$Backend = $false,
    [switch]$Frontend = $false,
    [switch]$Both = $false
)

$ErrorActionPreference = "Continue"
$repoRoot = "C:\LWC_Prod\log-repo"

if ($Both) {
    $Backend = $true
    $Frontend = $true
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  PHASE 1 CLEANUP - LOCAL ONLY" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

if ($Backend) {
    Write-Host "[BACKEND] Starting cleanup..." -ForegroundColor Yellow
    
    # Remove obj folders
    Write-Host "  Removing obj/ folders..." -ForegroundColor Gray
    $objFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter obj -ErrorAction SilentlyContinue
    $objCount = ($objFolders | Measure-Object).Count
    Write-Host "  Found: $objCount folders"
    
    $removed = 0
    foreach ($folder in $objFolders) {
        Remove-Item $folder.FullName -Recurse -Force -ErrorAction SilentlyContinue
        $removed++
        if ($removed % 100 -eq 0) {
            Write-Host "    Progress: $removed/$objCount"
        }
    }
    Write-Host "  [OK] Removed $removed obj/ folders" -ForegroundColor Green
    
    # Remove bin folders
    Write-Host "  Removing bin/ folders..." -ForegroundColor Gray
    $binFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter bin -ErrorAction SilentlyContinue
    $binCount = ($binFolders | Measure-Object).Count
    Write-Host "  Found: $binCount folders"
    
    $removed = 0
    foreach ($folder in $binFolders) {
        Remove-Item $folder.FullName -Recurse -Force -ErrorAction SilentlyContinue
        $removed++
        if ($removed % 100 -eq 0) {
            Write-Host "    Progress: $removed/$binCount"
        }
    }
    Write-Host "  [OK] Removed $removed bin/ folders" -ForegroundColor Green
    
    # Remove packages folders
    Write-Host "  Removing packages/ folders..." -ForegroundColor Gray
    $packagesFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter packages -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -notlike "*\node_modules\*" }
    $packagesCount = ($packagesFolders | Measure-Object).Count
    Write-Host "  Found: $packagesCount folders"
    
    $removed = 0
    foreach ($folder in $packagesFolders) {
        Write-Host "    Removing: $($folder.FullName)"
        Remove-Item $folder.FullName -Recurse -Force -ErrorAction SilentlyContinue
        $removed++
    }
    Write-Host "  [OK] Removed $removed packages/ folders" -ForegroundColor Green
    
    Write-Host "`n[BACKEND] Cleanup complete!`n" -ForegroundColor Green
}

if ($Frontend) {
    Write-Host "[FRONTEND] Starting cleanup..." -ForegroundColor Yellow
    
    # Remove node_modules folders
    Write-Host "  Removing node_modules/ folders..." -ForegroundColor Gray
    $nodeModulesFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter node_modules -ErrorAction SilentlyContinue
    $nodeModulesCount = ($nodeModulesFolders | Measure-Object).Count
    Write-Host "  Found: $nodeModulesCount folders"
    
    $removed = 0
    foreach ($folder in $nodeModulesFolders) {
        Write-Host "    Removing: $($folder.FullName)"
        Remove-Item $folder.FullName -Recurse -Force -ErrorAction SilentlyContinue
        $removed++
    }
    Write-Host "  [OK] Removed $removed node_modules/ folders" -ForegroundColor Green
    
    # Remove dist folders
    Write-Host "  Removing dist/ folders..." -ForegroundColor Gray
    $distFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter dist -ErrorAction SilentlyContinue
    $distCount = ($distFolders | Measure-Object).Count
    Write-Host "  Found: $distCount folders"
    
    $removed = 0
    foreach ($folder in $distFolders) {
        Remove-Item $folder.FullName -Recurse -Force -ErrorAction SilentlyContinue
        $removed++
    }
    Write-Host "  [OK] Removed $removed dist/ folders" -ForegroundColor Green
    
    # Remove .angular cache
    Write-Host "  Removing .angular/ cache folders..." -ForegroundColor Gray
    $angularFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter .angular -ErrorAction SilentlyContinue
    $angularCount = ($angularFolders | Measure-Object).Count
    Write-Host "  Found: $angularCount folders"
    
    $removed = 0
    foreach ($folder in $angularFolders) {
        Remove-Item $folder.FullName -Recurse -Force -ErrorAction SilentlyContinue
        $removed++
    }
    Write-Host "  [OK] Removed $removed .angular/ folders" -ForegroundColor Green
    
    Write-Host "`n[FRONTEND] Cleanup complete!`n" -ForegroundColor Green
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ALL CLEANUP COMPLETE" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan
Write-Host "NOTE: All changes are LOCAL only - nothing pushed to git" -ForegroundColor Yellow

