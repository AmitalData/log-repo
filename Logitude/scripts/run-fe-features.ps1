Param(
	[string] $ProjectPath = "..\..\AngularModules\AngularModules",
	[string] $Project, # maps to Jest --selectProjects
	[int] $MaxWorkers,
	[int] $MaxOldSpaceMB,
	[string] $Mode, # sequential | parallelAll
	[switch] $Detailed,
	[switch] $EnableJUnit,
	[switch] $Progress,
	[switch] $DetectOpenHandles,
	[switch] $ForceCleanInstall,
	[switch] $NoInstall
)

$ErrorActionPreference = "Stop"

function Write-Section([string] $message) {
	Write-Host ""
	Write-Host "=== $message ===" -ForegroundColor Cyan
}

function Assert-Path([string] $path, [string] $what) {
	if (!(Test-Path -Path $path)) {
		Write-Error "$what not found at path: $path"
		exit 1
	}
}

Push-Location $PSScriptRoot
try {
	# Resolve project directory
	$resolvedProjectPath = Resolve-Path -Path $ProjectPath
	Assert-Path $resolvedProjectPath "Frontend project directory"
	Set-Location $resolvedProjectPath

	Write-Section "Project"
	Write-Host "Using project at: $resolvedProjectPath"

	# Clean previous test outputs
	Write-Section "Clean previous results"
	foreach ($p in @("test-results","coverage-jest")) {
		if (Test-Path $p) {
			Write-Host "Removing: $p"
			Remove-Item -Recurse -Force $p -ErrorAction SilentlyContinue
		}
	}

	# Ensure package.json exists
	Assert-Path "package.json" "package.json"

	# Install dependencies unless explicitly skipped
	if (-not $NoInstall) {
		$needInstall = $ForceCleanInstall -or -not (Test-Path "node_modules")
		if ($needInstall) {
			Write-Section "Installing dependencies (npm ci)"
			npm ci
		} else {
			Write-Section "Dependencies already present (node_modules). Skipping install"
		}
	} else {
		Write-Section "Skipping dependency installation as requested"
	}

	# Build arguments for Node runner
	$nodeRunner = "tools\jest-runner.js"
	Assert-Path $nodeRunner "Node Jest runner"

	$args = @()
	if ($Project) { $args += @("--project", $Project) }
	if ($MaxWorkers) { $args += @("--maxWorkers", "$MaxWorkers") }
	if ($MaxOldSpaceMB) { $args += @("--maxOldSpaceMB", "$MaxOldSpaceMB") }
	if ($Mode) { $args += @("--mode", $Mode) }
	if ($Detailed) { $args += @("--detailed") }
	if ($EnableJUnit) { $args += @("--enableJUnit") }
	if ($Progress) { $args += @("--progress") }
	if ($DetectOpenHandles) { $args += @("--detectOpenHandles") }

	Write-Section "Running FE unit tests via Node runner"
	Write-Host "node $nodeRunner $($args -join ' ')"

	node $nodeRunner @args
	if ($LASTEXITCODE -ne 0) {
		Write-Error "Frontend unit tests failed with exit code $LASTEXITCODE"
		exit $LASTEXITCODE
	} else {
		Write-Host "Frontend unit tests completed successfully." -ForegroundColor Green
	}
}
finally {
	Pop-Location
}


