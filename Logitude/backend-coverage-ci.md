# Backend Coverage Automation

## Test Runner Configuration

- Run settings: `backend.coverage.runsettings` filters code coverage to business assemblies (`Logitude.*.BL.dll`, `Simplog.Data.dll`) and excludes generated artefacts under `Generated/` and T4 output.
- Use `Release` builds to avoid debug-only branches and ensure instrumentation compatibility.

```powershell
msbuild Logitude2-5.sln /t:Build /p:Configuration=Release

vstest.console.exe `
  Library\Bin\Logitude.Test.dll `
  Library\Bin\Logitude.CRMTests.dll `
  /Settings:backend.coverage.runsettings `
  /EnableCodeCoverage `
  /InIsolation
```

> Replace `vstest.console.exe` with the Visual Studio or `Microsoft.TestPlatform` CLI path available on the build agent. SpecFlow suites can be appended as required.

## CI Pipeline (Azure DevOps YAML fragment)

```yaml
- task: VSBuild@1
  inputs:
    solution: 'Logitude2-5.sln'
    msbuildArgs: '/p:Configuration=Release'

- task: VSTest@2
  inputs:
    testAssemblyVer2: |
      Library\Bin\Logitude.Test.dll
      Library\Bin\Logitude.CRMTests.dll
    runInParallel: false
    runSettingsFile: backend.coverage.runsettings
    codeCoverageEnabled: true

- script: |
    reportgenerator "-reports:$(Agent.TempDirectory)/**/coverage.cobertura.xml" `
                    "-targetdir:$(Build.SourcesDirectory)/coverage-report" `
                    "-reporttypes:Cobertura;HtmlSummary"
  displayName: 'Generate coverage report'
```

## Threshold Enforcement

| Metric | Target | Enforcement |
| --- | --- | --- |
| Overall backend coverage | ≥ 45% (Phase 1) | Fail pipeline if `CoverageSummary.xml` total line coverage &lt; 45%. |
| Accounting module | ≥ 60% | Parse `Cobertura` report for module namespace `Logitude.Accounting.BL`. |
| Customs module | ≥ 55% | Same approach using filtered XPath on Cobertura XML. |
| Shipment order module | ≥ 50% | Coverage gate for `Logitude.ShipmentOrderModule.BL`. |

Example PowerShell snippet to fail build:

```powershell
[xml]$cobertura = Get-Content coverage-report\Cobertura.xml
$overall = [double]$cobertura.coverage.'line-rate' * 100
if ($overall -lt 45) { throw \"Coverage gate failed: $overall% (target 45%)\" }
```

## Governance Checklist

- Store coverage artefacts (`Cobertura.xml`, HTML summary) as build artefacts for release audits.
- Add coverage badges to engineering dashboard once pipeline published.
- Review module-level thresholds quarterly; raise Accounting/Customs to 70% once new suites mature.
- Update `backend-baseline.md` when coverage gates change or new modules are prioritised.

## Outstanding Actions

1. Install Visual Studio Test Platform (or `dotnet tool install --global Microsoft.TestPlatform`) on agent to enable coverage runs.
2. Integrate new unit test suites (`Accounting`, `Customs`, `Shipments`) into nightly build and smoke pipeline.
3. Expand run-settings exclusion list when additional generated folders (e.g., `EntityLists/Generated`) are identified.


