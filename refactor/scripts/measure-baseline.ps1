# Measure baseline metrics before cleanup
$repoRoot = "C:\LWC_Prod\log-repo"
$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"

Write-Host "📊 Measuring baseline metrics..." -ForegroundColor Cyan

$metrics = @{
    Timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    RepositoryPath = $repoRoot
}

# Repository size
Write-Host "  Calculating repository size..." -ForegroundColor Gray
try {
    $size = (Get-ChildItem -Path $repoRoot -Recurse -ErrorAction SilentlyContinue | 
             Measure-Object -Property Length -Sum -ErrorAction SilentlyContinue).Sum
    $metrics.RepositorySizeGB = [math]::Round($size / 1GB, 2)
    Write-Host "  ✅ Size: $($metrics.RepositorySizeGB) GB" -ForegroundColor Green
} catch {
    $metrics.RepositorySizeGB = 0
    Write-Host "  ⚠️ Could not measure size" -ForegroundColor Yellow
}

# File count
Write-Host "  Counting files..." -ForegroundColor Gray
try {
    $fileCount = (Get-ChildItem -Path $repoRoot -Recurse -File -ErrorAction SilentlyContinue | Measure-Object).Count
    $metrics.TotalFiles = $fileCount
    Write-Host "  ✅ Files: $fileCount" -ForegroundColor Green
} catch {
    $metrics.TotalFiles = 0
    Write-Host "  ⚠️ Could not count files" -ForegroundColor Yellow
}

# obj folders
Write-Host "  Counting obj/ folders..." -ForegroundColor Gray
$objFolders = (Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter obj -ErrorAction SilentlyContinue | Measure-Object).Count
$metrics.ObjFolders = $objFolders
Write-Host "  ✅ obj/ folders: $objFolders" -ForegroundColor Green

# bin folders
Write-Host "  Counting bin/ folders..." -ForegroundColor Gray
$binFolders = (Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter bin -ErrorAction SilentlyContinue | Measure-Object).Count
$metrics.BinFolders = $binFolders
Write-Host "  ✅ bin/ folders: $binFolders" -ForegroundColor Green

# packages folders
Write-Host "  Counting packages/ folders..." -ForegroundColor Gray
$packagesFolders = (Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter packages -ErrorAction SilentlyContinue | 
                    Where-Object { $_.FullName -notlike "*\node_modules\*" } | Measure-Object).Count
$metrics.PackagesFolders = $packagesFolders
Write-Host "  ✅ packages/ folders: $packagesFolders" -ForegroundColor Green

# node_modules folders
Write-Host "  Counting node_modules/ folders..." -ForegroundColor Gray
$nodeModulesFolders = (Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter node_modules -ErrorAction SilentlyContinue | Measure-Object).Count
$metrics.NodeModulesFolders = $nodeModulesFolders
Write-Host "  ✅ node_modules/ folders: $nodeModulesFolders" -ForegroundColor Green

# dist folders
Write-Host "  Counting dist/ folders..." -ForegroundColor Gray
$distFolders = (Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter dist -ErrorAction SilentlyContinue | Measure-Object).Count
$metrics.DistFolders = $distFolders
Write-Host "  ✅ dist/ folders: $distFolders" -ForegroundColor Green

# Save metrics
$metricsFile = "$repoRoot\refactor\metrics\baseline-$timestamp.json"
$metrics | ConvertTo-Json | Out-File $metricsFile -Encoding UTF8

Write-Host "`n✅ Baseline metrics saved to: $metricsFile" -ForegroundColor Green
Write-Host "`n📊 Summary:" -ForegroundColor Cyan
$metrics | Format-Table -AutoSize

return $metricsFile

