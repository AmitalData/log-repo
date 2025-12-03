<#
    CI/CD Test Runner Script for Jenkins
    Runs tests with code coverage and outputs results in Jenkins-friendly format

    Usage:
        .\run-tests-ci.ps1 [-CoverageThreshold <percentage>] [-OutputDir <path>]

    Exit Codes:
        0 - All tests passed and coverage threshold met
        1 - Tests failed or coverage threshold not met
#>

param(
    [int]$CoverageThreshold = 0,  # Minimum coverage percentage (0-100)
    [string]$OutputDir = '.\TestResults',
    [string]$Configuration = 'Debug',
    [int]$ParallelWorkers = 0,  # Number of parallel workers (0 = auto-detect CPU count, -1 = disable parallel)
    [switch]$EnableParallel = $false  # Enable parallel test execution
)

$ErrorActionPreference = 'Stop'

# Set working directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

# Start execution timer
$script:startTime = Get-Date

# Create output directory
if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "CI/CD Test Runner" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Configuration: $Configuration" -ForegroundColor Green
Write-Host "Coverage Threshold: $CoverageThreshold percent" -ForegroundColor Green
Write-Host "Output Directory: $OutputDir" -ForegroundColor Green

# Determine parallel execution settings
$cpuCount = (Get-WmiObject Win32_Processor | Measure-Object -Property NumberOfLogicalProcessors -Sum).Sum
if ($ParallelWorkers -eq 0) {
    $ParallelWorkers = $cpuCount
}
if ($EnableParallel -and $ParallelWorkers -gt 0) {
    Write-Host "Parallel Execution: Enabled ($ParallelWorkers workers)" -ForegroundColor Green
} else {
    Write-Host "Parallel Execution: Disabled" -ForegroundColor Yellow
    $ParallelWorkers = 1
}
Write-Host ""

# Find vstest.console.exe
$vstest = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.VisualStudio.PackageGroup.TestTools.Core -find "Common7\IDE\Extensions\TestPlatform\vstest.console.exe" | Select-Object -First 1

if (-not $vstest -or -not (Test-Path $vstest)) {
    Write-Error "vstest.console.exe not found. Please install Visual Studio Test Tools."
    exit 1
}

Write-Host "Using: $vstest" -ForegroundColor Green
Write-Host ""

# Check if coverlet.msbuild is available (MSBuild integration) - do this early to determine if we need DLL check
# Check both package layout formats:
# 1. PackageName\Version\ format: packages\coverlet.msbuild\6.0.0\build\...
# 2. PackageName.Version format: packages\coverlet.msbuild.6.0.0\build\...
# Note: Targets file is directly in build\ directory, not in netstandard2.0 subdirectory
$coverletMsbuild = $null

# Try PackageName\Version format first (with netstandard2.0 subdirectory)
$coverletMsbuildPath1 = '.\packages\coverlet.msbuild\*\build\netstandard2.0\coverlet.msbuild.targets'
$coverletMsbuild = Get-ChildItem -Path $coverletMsbuildPath1 -ErrorAction SilentlyContinue | Select-Object -First 1

# If not found, try PackageName.Version format (with netstandard2.0 subdirectory)
if (-not $coverletMsbuild) {
    $coverletMsbuildPath2 = '.\packages\coverlet.msbuild.*\build\netstandard2.0\coverlet.msbuild.targets'
    $coverletMsbuild = Get-ChildItem -Path $coverletMsbuildPath2 -ErrorAction SilentlyContinue | Select-Object -First 1
}

# If still not found, try without netstandard2.0 subdirectory (PackageName\Version format)
if (-not $coverletMsbuild) {
    $coverletMsbuildPath3 = '.\packages\coverlet.msbuild\*\build\coverlet.msbuild.targets'
    $coverletMsbuild = Get-ChildItem -Path $coverletMsbuildPath3 -ErrorAction SilentlyContinue | Select-Object -First 1
}

# If still not found, try without netstandard2.0 subdirectory (PackageName.Version format)
if (-not $coverletMsbuild) {
    $coverletMsbuildPath4 = '.\packages\coverlet.msbuild.*\build\coverlet.msbuild.targets'
    $coverletMsbuild = Get-ChildItem -Path $coverletMsbuildPath4 -ErrorAction SilentlyContinue | Select-Object -First 1
}

# Also check for coverlet.console (standalone tool)
$coverletConsolePath = '.\packages\coverlet.console\*\tools\net*\coverlet.exe'
$coverletConsole = Get-ChildItem -Path $coverletConsolePath -ErrorAction SilentlyContinue | Select-Object -First 1

$useCoverlet = $false
$coverlet = $null

if ($coverletMsbuild -and (Test-Path $coverletMsbuild.FullName)) {
    # Use MSBuild integration (preferred)
    $useCoverlet = $true
} elseif ($coverletConsole -and (Test-Path $coverletConsole.FullName)) {
    # Use console tool as fallback
    $useCoverlet = $true
    $coverlet = $coverletConsole
}

# Find test DLL (will be built by MSBuild if using coverlet.msbuild with VSTest target)
$testDll = 'Library\Bin\Logitude.UnitTest.dll'
if (-not (Test-Path $testDll)) {
    $testDll = "Logitude.UnitTest\bin\$Configuration\Logitude.UnitTest.dll"
}

# Only check for DLL if not using coverlet.msbuild with MSBuild VSTest (MSBuild will build it)
$checkDll = $true
if ($useCoverlet -and $coverletMsbuild) {
    # Will use MSBuild VSTest which builds automatically
    $checkDll = $false
}

if ($checkDll -and -not (Test-Path $testDll)) {
    Write-Error "Test DLL not found: $testDll"
    exit 1
}

if ($checkDll) {
    Write-Host "Test DLL: $testDll" -ForegroundColor Green
    Write-Host ""
}

# Coverage file paths
$coverageFile = Join-Path $OutputDir "coverage.cobertura.xml"
$coverageJson = Join-Path $OutputDir "coverage.json"
$trxFile = Join-Path $OutputDir "test-results.trx"
$summaryFile = Join-Path $OutputDir "test-summary.txt"
$jenkinsFile = Join-Path $OutputDir "jenkins-status.txt"

# Restore NuGet packages to ensure coverlet.msbuild is available
Write-Host "Restoring NuGet packages..." -ForegroundColor Cyan

# Use MSBuild restore (more reliable, doesn't require NuGet.exe)
$programFilesX86 = ${env:ProgramFiles(x86)}
$vswherePath = Join-Path $programFilesX86 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswherePath -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
if (-not $msbuild) {
    $msbuild = 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe'
}

if ($msbuild -and (Test-Path $msbuild)) {
    Write-Host "  Using MSBuild restore..." -ForegroundColor Gray
    $testProjectPath = 'Logitude.UnitTest\Logitude.UnitTest.csproj'
    $restoreResult = & $msbuild $testProjectPath /t:Restore /p:RestorePackagesConfig=true /nologo /v:minimal 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  [OK] Packages restored successfully" -ForegroundColor Green
    } else {
        Write-Host "  [WARN] Package restore had issues (continuing anyway)" -ForegroundColor Yellow
    }
} else {
    Write-Host "  [WARN] MSBuild not found - packages may need manual restore" -ForegroundColor Yellow
}

# Display coverlet status (already detected earlier)
if ($useCoverlet -and $coverletMsbuild) {
    Write-Host "Coverlet.msbuild found: $($coverletMsbuild.FullName)" -ForegroundColor Green
    Write-Host "Coverage will be collected during test execution via MSBuild." -ForegroundColor Green
} elseif ($useCoverlet -and $coverlet) {
    Write-Host "Coverlet.console found: $($coverlet.FullName)" -ForegroundColor Green
} else {
    Write-Host "Coverlet not found. Running tests without coverage collection." -ForegroundColor Yellow
    Write-Host "To enable coverage, install coverlet.msbuild NuGet package to Logitude.UnitTest project." -ForegroundColor Yellow
    Write-Host "  Option 1: Visual Studio -> Right-click project -> Manage NuGet Packages -> Install coverlet.msbuild" -ForegroundColor Gray
    Write-Host "  Option 2: Run .\install-coverlet.ps1 for installation instructions" -ForegroundColor Gray
}

Write-Host ""

# Run tests
Write-Host "Running tests..." -ForegroundColor Cyan
Write-Host ""

$testArgs = @(
    $testDll,
    "/Logger:console",
    "/Logger:trx;LogFileName=$trxFile",
    "/TestCaseFilter:FullyQualifiedName~MSTest"
)

# Create or update runsettings file for parallel execution
$runsettingsFile = Join-Path $scriptPath "test.runsettings"
if ($EnableParallel) {
    if (Test-Path $runsettingsFile) {
        # Update existing runsettings file
        [xml]$runsettings = Get-Content $runsettingsFile
        if ($ParallelWorkers -gt 0) {
            $runsettings.RunSettings.MSTest.Parallelize.Workers = $ParallelWorkers.ToString()
            $runsettings.RunSettings.RunConfiguration.MaxCpuCount = $ParallelWorkers.ToString()
        } else {
            $runsettings.RunSettings.MSTest.Parallelize.Workers = '0'
            $runsettings.RunSettings.RunConfiguration.MaxCpuCount = '0'
        }
        # Use ClassLevel scope for better isolation (tests in same class won't run in parallel)
        # Change to MethodLevel if tests are fully isolated
        $runsettings.RunSettings.MSTest.Parallelize.Scope = 'ClassLevel'
        $runsettings.Save($runsettingsFile)
    }
    $testArgs += "/Settings:$runsettingsFile"
    if ($ParallelWorkers -gt 0) {
        Write-Host "Parallel test execution enabled with $ParallelWorkers workers" -ForegroundColor Cyan
    } else {
        Write-Host "Parallel test execution enabled (auto-detect workers)" -ForegroundColor Cyan
    }
} else {
    Write-Host "Parallel test execution disabled" -ForegroundColor Yellow
}

if ($useCoverlet -and $coverlet) {
    # Run with coverlet.console for coverage
    # Use base path without extension for output (coverlet adds extensions based on format)
    $coverageBasePath = Join-Path $OutputDir "coverage"
    $quote = '"'
    $coverletArgs = @(
        $testDll,
        '--target', ($quote + $vstest + $quote),
        '--targetargs', ($quote + ($testArgs -join ' ') + $quote),
        '--format', 'cobertura',
        '--format', 'json',
        '--output', $coverageBasePath,
        '--exclude-by-attribute', '[*]*',
        '--exclude-by-file', '**/Migrations/**',
        '--exclude-by-file', '**/Generated/**'
    )
    
    Write-Host 'Running tests with coverage collection (coverlet.console)...' -ForegroundColor Cyan
    $coverageOutput = & $coverlet.FullName $coverletArgs 2>&1
    $testExitCode = $LASTEXITCODE
    
    # Coverlet generates files with extensions: coverage.cobertura.xml and coverage.json
    # Move/rename to expected locations if needed
    $generatedCobertura = $coverageBasePath + '.cobertura.xml'
    $generatedJson = $coverageBasePath + '.json'
    if (Test-Path $generatedCobertura) {
        Copy-Item $generatedCobertura $coverageFile -Force -ErrorAction SilentlyContinue
    }
    if (Test-Path $generatedJson) {
        Copy-Item $generatedJson $coverageJson -Force -ErrorAction SilentlyContinue
    }
    
    # Extract test output from coverlet output
    $testOutput = $coverageOutput
} else {
    # If coverlet.msbuild is installed, rebuild to instrument, then run vstest directly
    if ($useCoverlet -and $coverletMsbuild) {
        Write-Host 'Rebuilding test project with coverlet.msbuild instrumentation...' -ForegroundColor Cyan
        $testProjectPath = 'Logitude.UnitTest\Logitude.UnitTest.csproj'
        $coverageOutputPath = Join-Path $OutputDir 'coverage'
        
        # Rebuild with coverlet properties to instrument assemblies
        # Quote property values containing semicolons or commas to prevent MSBuild from splitting them
        $buildArgs = @(
            $testProjectPath,
            '/t:Rebuild',
            "/p:Configuration=$Configuration",
            '/p:CollectCoverage=true',
            '/p:CoverletOutputFormat="cobertura;json"',
            "/p:CoverletOutput=$coverageOutputPath",
            '/p:ExcludeByAttribute="Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute"',
            '/p:ExcludeByFile="**/Migrations/**;**/Generated/**"',
            '/nologo',
            '/v:minimal'
        )
        $buildOutput = & $msbuild $buildArgs 2>&1
        $buildExitCode = $LASTEXITCODE
        if ($buildExitCode -ne 0) {
            Write-Warning 'Build with coverlet instrumentation had issues, but continuing with test execution...'
        } else {
            Write-Host '[OK] Project rebuilt successfully with coverlet instrumentation' -ForegroundColor Green
        }
        
        # Update test DLL path after rebuild (should be in Library\Bin based on project OutputPath)
        $testDll = 'Library\Bin\Logitude.UnitTest.dll'
        if (-not (Test-Path $testDll)) {
            $testDll = "Logitude.UnitTest\bin\$Configuration\Logitude.UnitTest.dll"
        }
        if (-not (Test-Path $testDll)) {
            Write-Error "Test DLL not found after rebuild: $testDll"
            exit 1
        }
        
        # Rebuild testArgs with correct DLL path
        $testArgs = @(
            $testDll,
            "/Logger:console",
            "/Logger:trx;LogFileName=$trxFile",
            "/TestCaseFilter:FullyQualifiedName~MSTest"
        )
        if ($EnableParallel -and (Test-Path $runsettingsFile)) {
            $testArgs += "/Settings:$runsettingsFile"
        }
        
        # Now run tests directly with vstest - coverlet should collect coverage during execution
        Write-Host 'Running tests with instrumented assemblies...' -ForegroundColor Cyan
        $testOutput = & $vstest $testArgs 2>&1
        $testExitCode = $LASTEXITCODE
    } else {
        # Run tests without coverage
        Write-Host 'Running tests...' -ForegroundColor Cyan
        $testOutput = & $vstest $testArgs 2>&1
        $testExitCode = $LASTEXITCODE
    }
    
    # If coverlet.msbuild is installed, coverage files should be generated after test execution
    # Check for coverage files in the configured output location and common locations
    if ($useCoverlet -and $coverletMsbuild) {
        $coverageOutputPath = Join-Path $OutputDir 'coverage'
        $possibleCoverageFiles = @(
            ($coverageOutputPath + '.cobertura.xml'),
            ($coverageOutputPath + '.json'),
            (Join-Path $OutputDir 'coverage.cobertura.xml'),
            (Join-Path $OutputDir 'coverage.json'),
            'coverage.cobertura.xml',
            'coverage.json',
            '.\coverage.cobertura.xml',
            '.\coverage.json',
            (Join-Path $scriptPath 'coverage.cobertura.xml'),
            (Join-Path $scriptPath 'coverage.json')
        )
        
        $foundAny = $false
        foreach ($file in $possibleCoverageFiles) {
            if (Test-Path $file) {
                $destFile = Split-Path $file -Leaf
                $destPath = Join-Path $OutputDir $destFile
                Copy-Item $file $destPath -Force -ErrorAction SilentlyContinue
                Write-Host ('Found coverage file: ' + $file) -ForegroundColor Green
                $foundAny = $true
            }
        }
        
        if (-not $foundAny) {
            Write-Warning 'Coverage files not found after test execution. Make sure coverlet.msbuild is properly configured in the project file.'
        }
    }
}

# Parse test results from output
$passedCount = 0
$failedCount = 0
$skippedCount = 0
$totalCount = 0

# Try to parse from console output
$testOutput | ForEach-Object {
    $line = $_.ToString()
    if ($line -match 'Passed\s+(\w+)\s+\[') {
        $passedCount++
    }
    if ($line -match 'Failed\s+(\w+)\s+\[') {
        $failedCount++
    }
    if ($line -match 'Skipped\s+(\w+)\s+\[') {
        $skippedCount++
    }
    if ($line -match 'Total tests:\s+(\d+)') {
        $totalCount = [int]$matches[1]
    }
    if ($line -match 'Passed:\s+(\d+)') {
        $passedCount = [int]$matches[1]
    }
    if ($line -match 'Failed:\s+(\d+)') {
        $failedCount = [int]$matches[1]
    }
    if ($line -match 'Skipped:\s+(\d+)') {
        $skippedCount = [int]$matches[1]
    }
}

# If we couldn't parse from output, try to get from TRX file
if ($totalCount -eq 0 -and (Test-Path $trxFile)) {
    $trxContent = Get-Content $trxFile -Raw
    if ($trxContent -match 'total=(\d+)') {
        $totalCount = [int]$matches[1]
    }
    if ($trxContent -match 'passed=(\d+)') {
        $passedCount = [int]$matches[1]
    }
    if ($trxContent -match 'failed=(\d+)') {
        $failedCount = [int]$matches[1]
    }
    if ($trxContent -match 'executed=(\d+)') {
        $executedCount = [int]$matches[1]
        $skippedCount = $totalCount - $executedCount
    }
}

# Calculate coverage if available
$coveragePercentage = 0
$moduleCoverage = @{}
$coverageDetails = @{}

if ($useCoverlet -and (Test-Path $coverageJson)) {
    try {
        $coverageData = Get-Content $coverageJson | ConvertFrom-Json
        
        # Overall coverage
        if ($coverageData.summary) {
            $covered = $coverageData.summary.covered
            $total = $coverageData.summary.total
            if ($total -gt 0) {
                $coveragePercentage = [math]::Round(($covered / $total) * 100, 2)
            }
        }
        
        # Per-module coverage
        if ($coverageData.modules) {
            foreach ($module in $coverageData.modules) {
                $moduleName = $module.name
                $moduleCovered = 0
                $moduleTotal = 0
                
                if ($module.summary) {
                    $moduleCovered = $module.summary.covered
                    $moduleTotal = $module.summary.total
                } elseif ($module.covered) {
                    $moduleCovered = $module.covered
                    $moduleTotal = $module.total
                }
                
                if ($moduleTotal -gt 0) {
                    $modulePct = [math]::Round(($moduleCovered / $moduleTotal) * 100, 2)
                    $moduleCoverage[$moduleName] = $modulePct
                    $coverageDetails[$moduleName] = @{
                        Covered = $moduleCovered
                        Total = $moduleTotal
                        Percentage = $modulePct
                    }
                }
            }
        }
    } catch {
        Write-Warning ('Failed to parse coverage JSON: ' + $_.ToString())
    }
}

# Calculate execution time
$endTime = Get-Date
$executionTime = $endTime - $script:startTime
$executionTimeSeconds = [math]::Round($executionTime.TotalSeconds, 2)
$executionTimeFormatted = '{0:D2}:{1:D2}:{2:D2}' -f $executionTime.Hours, $executionTime.Minutes, $executionTime.Seconds

# Determine overall status
$testsPassed = ($failedCount -eq 0)
$coverageMet = ($coveragePercentage -ge $CoverageThreshold)
$overallPass = $testsPassed -and $coverageMet

# Build module coverage section
$moduleCoverageSection = ""
if ($moduleCoverage.Count -gt 0) {
    $newline = [Environment]::NewLine
    $moduleCoverageSection = $newline + 'Per-Module Coverage:' + $newline
    foreach ($moduleName in $moduleCoverage.Keys | Sort-Object) {
        $modulePct = $moduleCoverage[$moduleName]
        $moduleInfo = $coverageDetails[$moduleName]
        $coveredLines = $moduleInfo.Covered
        $totalLines = $moduleInfo.Total
        $lineInfo = $coveredLines.ToString() + '/' + $totalLines.ToString() + ' lines'
        $moduleLine = '  ' + $moduleName + ' : ' + $modulePct.ToString() + ' percent ' + $lineInfo
        $moduleCoverageSection += $moduleLine + [Environment]::NewLine
    }
}

# Write summary
$execTimeLine = 'Execution Time:  ' + $executionTimeFormatted
$coverageStatusText = if ($coverageMet) { 'PASS' } else { 'FAIL' }
$overallStatusText = if ($overallPass) { 'PASS' } else { 'FAIL' }
$newline = [Environment]::NewLine
$summary = '========================================' + $newline
$summary += 'TEST EXECUTION SUMMARY' + $newline
$summary += '========================================' + $newline
$summary += $execTimeLine + $newline + $newline
$summary += 'Total Tests:     ' + $totalCount.ToString() + $newline
$summary += 'Passed:          ' + $passedCount.ToString() + $newline
$summary += 'Failed:          ' + $failedCount.ToString() + $newline
$summary += 'Skipped:         ' + $skippedCount.ToString() + $newline + $newline
$summary += 'Overall Code Coverage:   ' + $coveragePercentage.ToString() + ' percent' + $newline
$summary += 'Threshold:               ' + $CoverageThreshold.ToString() + ' percent' + $newline
$summary += 'Coverage Status:         ' + $coverageStatusText + $newline
$summary += $moduleCoverageSection
$summary += 'Overall Status:  ' + $overallStatusText + $newline
$summary += '========================================' + $newline


$summaryColor = if ($overallPass) { 'Green' } else { 'Red' }
Write-Host $summary -ForegroundColor $summaryColor

# Write summary to file
$summary | Out-File -FilePath $summaryFile -Encoding UTF8

# Write Jenkins-friendly status file
$testResultText = if ($testsPassed) { 'PASS' } else { 'FAIL' }
$coverageResultText = if ($coverageMet) { 'PASS' } else { 'FAIL' }
$overallResultText = if ($overallPass) { 'PASS' } else { 'FAIL' }
$parallelEnabledText = if ($EnableParallel -and $ParallelWorkers -gt 1) { 'true' } else { 'false' }
$newline = [Environment]::NewLine
$jenkinsStatus = 'TEST_RESULT=' + $testResultText + $newline
$jenkinsStatus += 'COVERAGE_RESULT=' + $coverageResultText + $newline
$jenkinsStatus += 'OVERALL_RESULT=' + $overallResultText + $newline
$jenkinsStatus += 'TOTAL_TESTS=' + $totalCount.ToString() + $newline
$jenkinsStatus += 'PASSED_TESTS=' + $passedCount.ToString() + $newline
$jenkinsStatus += 'FAILED_TESTS=' + $failedCount.ToString() + $newline
$jenkinsStatus += 'SKIPPED_TESTS=' + $skippedCount.ToString() + $newline
$jenkinsStatus += 'COVERAGE_PERCENTAGE=' + $coveragePercentage.ToString() + $newline
$jenkinsStatus += 'COVERAGE_THRESHOLD=' + $CoverageThreshold.ToString() + $newline
$jenkinsStatus += 'EXECUTION_TIME_SECONDS=' + $executionTimeSeconds.ToString() + $newline
$jenkinsStatus += 'EXECUTION_TIME_FORMATTED=' + $executionTimeFormatted + $newline
$jenkinsStatus += 'PARALLEL_ENABLED=' + $parallelEnabledText + $newline
$jenkinsStatus += 'PARALLEL_WORKERS=' + $ParallelWorkers.ToString() + $newline

# Write module coverage to separate file (JSON format for easy parsing)
if ($moduleCoverage.Count -gt 0) {
    $moduleCoverageJson = @{
        modules = @()
    }
    foreach ($moduleName in $moduleCoverage.Keys | Sort-Object) {
        $moduleInfo = $coverageDetails[$moduleName]
        $moduleCoverageJson.modules += @{
            name = $moduleName
            coverage = $moduleCoverage[$moduleName]
            covered = $moduleInfo.Covered
            total = $moduleInfo.Total
        }
    }
    $moduleCoverageFileName = 'module-coverage.json'
    $moduleCoverageFile = Join-Path $OutputDir $moduleCoverageFileName
    $moduleCoverageJson | ConvertTo-Json -Depth 10 | Out-File -FilePath $moduleCoverageFile -Encoding UTF8
}

$jenkinsStatus | Out-File -FilePath $jenkinsFile -Encoding UTF8

# Write coverage percentage to a simple file for easy parsing
$coveragePctFileName = 'coverage-percentage.txt'
$coveragePctFile = Join-Path $OutputDir $coveragePctFileName
$coveragePercentage.ToString() | Out-File -FilePath $coveragePctFile -Encoding UTF8 -NoNewline

# Output results in a format Jenkins can easily parse
Write-Host ""
Write-Host 'Results written to:' -ForegroundColor Cyan
Write-Host ('  Summary: ' + $summaryFile) -ForegroundColor Gray
Write-Host ('  Status: ' + $jenkinsFile) -ForegroundColor Gray
Write-Host ('  Coverage: ' + $coverageFile) -ForegroundColor Gray
Write-Host ('  TRX: ' + $trxFile) -ForegroundColor Gray
Write-Host ""

# Exit with appropriate code
if ($overallPass) {
    $passMsg = '[PASS] All checks passed!'
    Write-Host $passMsg -ForegroundColor Green
    exit 0
} else {
    $failMsg = '[FAIL] Some checks failed!'
    Write-Host $failMsg -ForegroundColor Red
    if (-not $testsPassed) {
        $failedMsg = '  - Tests failed: ' + $failedCount.ToString()
        Write-Host $failedMsg -ForegroundColor Red
    }
    if (-not $coverageMet) {
        $coverageMsg = '  - Coverage below threshold: ' + $coveragePercentage + ' percent (required: ' + $CoverageThreshold + ' percent)'
        Write-Host $coverageMsg -ForegroundColor Red
    }
    exit 1
}


