# Frontend Cleanup Script - Phase 1
param(
    [switch]$DryRun = $false
)

$repoRoot = "C:\LWC_Prod\log-repo"
$logFile = "$repoRoot\refactor\logs\frontend-cleanup-$(Get-Date -Format 'yyyyMMdd-HHmmss').log"
$checkpointFile = "$repoRoot\refactor\checkpoints\phase1-fe-checkpoint.json"

function Write-Log {
    param([string]$Message, [string]$Color = "White")
    $timestamp = Get-Date -Format "HH:mm:ss"
    $logMessage = "[$timestamp] $Message"
    Write-Host $logMessage -ForegroundColor $Color
    Add-Content -Path $logFile -Value $logMessage
}

function Update-Checkpoint {
    param([string]$StepId, [string]$Status, [hashtable]$Data = @{})
    
    $checkpoint = Get-Content $checkpointFile | ConvertFrom-Json
    $step = $checkpoint.steps | Where-Object { $_.id -eq $StepId }
    
    if ($step) {
        $step.status = $Status
        $step.timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
        
        foreach ($key in $Data.Keys) {
            $step | Add-Member -NotePropertyName $key -NotePropertyValue $Data[$key] -Force
        }
    }
    
    $checkpoint.lastUpdated = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $checkpoint | ConvertTo-Json -Depth 10 | Out-File $checkpointFile -Encoding UTF8
}

Write-Log "🧹 FRONTEND CLEANUP PHASE 1" -Color Cyan
Write-Log "Repository: $repoRoot"
Write-Log "Dry Run: $DryRun"
Write-Log "Log File: $logFile"
Write-Log ""

# Step 1.1-fe: Remove node_modules folders
Write-Log "📦 Step 1.1: Removing node_modules/ folders..." -Color Yellow
Update-Checkpoint -StepId "1.1-fe" -Status "in_progress"

$nodeModulesFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter node_modules -ErrorAction SilentlyContinue
$nodeModulesCount = ($nodeModulesFolders | Measure-Object).Count
Write-Log "  Found $nodeModulesCount node_modules/ folders"

if (-not $DryRun) {
    $removed = 0
    foreach ($folder in $nodeModulesFolders) {
        try {
            Write-Log "    Removing: $($folder.FullName)" -Color Gray
            Remove-Item $folder.FullName -Recurse -Force -ErrorAction Stop
            $removed++
        } catch {
            Write-Log "    ⚠️ Failed to remove: $($folder.FullName)" -Color Yellow
        }
    }
    Write-Log "  ✅ Removed $removed node_modules/ folders" -Color Green
    Update-Checkpoint -StepId "1.1-fe" -Status "completed" -Data @{ foldersRemoved = $removed }
} else {
    Write-Log "  [DRY RUN] Would remove $nodeModulesCount folders" -Color Yellow
    Update-Checkpoint -StepId "1.1-fe" -Status "completed" -Data @{ foldersRemoved = 0 }
}

# Step 1.2-fe: Remove dist folders
Write-Log "`n📦 Step 1.2: Removing dist/ folders..." -Color Yellow
Update-Checkpoint -StepId "1.2-fe" -Status "in_progress"

$distFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter dist -ErrorAction SilentlyContinue
$distCount = ($distFolders | Measure-Object).Count
Write-Log "  Found $distCount dist/ folders"

if (-not $DryRun) {
    $removed = 0
    foreach ($folder in $distFolders) {
        try {
            Remove-Item $folder.FullName -Recurse -Force -ErrorAction Stop
            $removed++
        } catch {
            Write-Log "    ⚠️ Failed to remove: $($folder.FullName)" -Color Yellow
        }
    }
    Write-Log "  ✅ Removed $removed dist/ folders" -Color Green
    Update-Checkpoint -StepId "1.2-fe" -Status "completed" -Data @{ foldersRemoved = $removed }
} else {
    Write-Log "  [DRY RUN] Would remove $distCount folders" -Color Yellow
    Update-Checkpoint -StepId "1.2-fe" -Status "completed" -Data @{ foldersRemoved = 0 }
}

# Step 1.3-fe: Remove .angular cache folders
Write-Log "`n📦 Step 1.3: Removing .angular/ cache folders..." -Color Yellow
Update-Checkpoint -StepId "1.3-fe" -Status "in_progress"

$angularCacheFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter .angular -ErrorAction SilentlyContinue
$angularCacheCount = ($angularCacheFolders | Measure-Object).Count
Write-Log "  Found $angularCacheCount .angular/ folders"

if (-not $DryRun) {
    $removed = 0
    foreach ($folder in $angularCacheFolders) {
        try {
            Remove-Item $folder.FullName -Recurse -Force -ErrorAction Stop
            $removed++
        } catch {
            Write-Log "    ⚠️ Failed to remove: $($folder.FullName)" -Color Yellow
        }
    }
    Write-Log "  ✅ Removed $removed .angular/ folders" -Color Green
    Update-Checkpoint -StepId "1.3-fe" -Status "completed" -Data @{ foldersRemoved = $removed }
} else {
    Write-Log "  [DRY RUN] Would remove $angularCacheCount folders" -Color Yellow
    Update-Checkpoint -StepId "1.3-fe" -Status "completed" -Data @{ foldersRemoved = 0 }
}

Write-Log "`n✅ FRONTEND CLEANUP COMPLETE!" -Color Green
Write-Log "Check log file: $logFile" -Color Cyan

