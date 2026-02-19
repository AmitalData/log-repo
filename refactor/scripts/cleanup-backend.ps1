# Backend Cleanup Script - Phase 1
param(
    [switch]$DryRun = $false
)

$repoRoot = "C:\LWC_Prod\log-repo"
$logFile = "$repoRoot\refactor\logs\backend-cleanup-$(Get-Date -Format 'yyyyMMdd-HHmmss').log"
$checkpointFile = "$repoRoot\refactor\checkpoints\phase1-be-checkpoint.json"

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

Write-Log "🧹 BACKEND CLEANUP PHASE 1" -Color Cyan
Write-Log "Repository: $repoRoot"
Write-Log "Dry Run: $DryRun"
Write-Log "Log File: $logFile"
Write-Log ""

# Step 1.1-be: Remove obj folders
Write-Log "📦 Step 1.1: Removing obj/ folders..." -Color Yellow
Update-Checkpoint -StepId "1.1-be" -Status "in_progress"

$objFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter obj -ErrorAction SilentlyContinue
$objCount = ($objFolders | Measure-Object).Count
Write-Log "  Found $objCount obj/ folders"

if (-not $DryRun) {
    $removed = 0
    foreach ($folder in $objFolders) {
        try {
            Remove-Item $folder.FullName -Recurse -Force -ErrorAction Stop
            $removed++
            if ($removed % 50 -eq 0) {
                Write-Log "    Removed $removed/$objCount..." -Color Gray
            }
        } catch {
            Write-Log "    ⚠️ Failed to remove: $($folder.FullName)" -Color Yellow
        }
    }
    Write-Log "  ✅ Removed $removed obj/ folders" -Color Green
    Update-Checkpoint -StepId "1.1-be" -Status "completed" -Data @{ foldersRemoved = $removed }
} else {
    Write-Log "  [DRY RUN] Would remove $objCount folders" -Color Yellow
    Update-Checkpoint -StepId "1.1-be" -Status "completed" -Data @{ foldersRemoved = 0 }
}

# Step 1.2-be: Remove bin folders
Write-Log "`n📦 Step 1.2: Removing bin/ folders..." -Color Yellow
Update-Checkpoint -StepId "1.2-be" -Status "in_progress"

$binFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter bin -ErrorAction SilentlyContinue
$binCount = ($binFolders | Measure-Object).Count
Write-Log "  Found $binCount bin/ folders"

if (-not $DryRun) {
    $removed = 0
    foreach ($folder in $binFolders) {
        try {
            Remove-Item $folder.FullName -Recurse -Force -ErrorAction Stop
            $removed++
            if ($removed % 50 -eq 0) {
                Write-Log "    Removed $removed/$binCount..." -Color Gray
            }
        } catch {
            Write-Log "    ⚠️ Failed to remove: $($folder.FullName)" -Color Yellow
        }
    }
    Write-Log "  ✅ Removed $removed bin/ folders" -Color Green
    Update-Checkpoint -StepId "1.2-be" -Status "completed" -Data @{ foldersRemoved = $removed }
} else {
    Write-Log "  [DRY RUN] Would remove $binCount folders" -Color Yellow
    Update-Checkpoint -StepId "1.2-be" -Status "completed" -Data @{ foldersRemoved = 0 }
}

# Step 1.3-be: Remove packages folders
Write-Log "`n📦 Step 1.3: Removing packages/ folders..." -Color Yellow
Update-Checkpoint -StepId "1.3-be" -Status "in_progress"

$packagesFolders = Get-ChildItem -Path $repoRoot -Recurse -Directory -Filter packages -ErrorAction SilentlyContinue |
    Where-Object { $_.FullName -notlike "*\node_modules\*" }
$packagesCount = ($packagesFolders | Measure-Object).Count
Write-Log "  Found $packagesCount packages/ folders"

if (-not $DryRun) {
    $removed = 0
    foreach ($folder in $packagesFolders) {
        try {
            Write-Log "    Removing: $($folder.FullName)" -Color Gray
            Remove-Item $folder.FullName -Recurse -Force -ErrorAction Stop
            $removed++
        } catch {
            Write-Log "    ⚠️ Failed to remove: $($folder.FullName)" -Color Yellow
        }
    }
    Write-Log "  ✅ Removed $removed packages/ folders" -Color Green
    Update-Checkpoint -StepId "1.3-be" -Status "completed" -Data @{ foldersRemoved = $removed }
} else {
    Write-Log "  [DRY RUN] Would remove $packagesCount folders" -Color Yellow
    Update-Checkpoint -StepId "1.3-be" -Status "completed" -Data @{ foldersRemoved = 0 }
}

Write-Log "`n✅ BACKEND CLEANUP COMPLETE!" -Color Green
Write-Log "Check log file: $logFile" -Color Cyan

