# Backend Unit Test Baseline – Logitude Backend

## Snapshot (2025-11-10)

- **Primary unit-test assembly**: `Logitude.Test` (MSTest, 499 methods decorated with `[TestMethod]`)
- **SpecFlow/BDD suites**: `Logitude.*Tests`, `Logitude.FullAccounting.Test` (cover key business flows but run as integration-level tests)
- **Code coverage tooling**: `vstest.console.exe` not available on host; no recent `.coverage` artefacts
- **Conclusion**: Only infrastructure/common-data paths enjoy true unit coverage; the majority of backend business services currently rely on higher-level SpecFlow suites.

## Coverage Inventory

| Backend module | Assembly | Existing tests | Test style | Notes |
| --- | --- | --- | --- | --- |
| Core framework | `Logitude.BL`, `Logitude.Infrastructure.BL` | `Logitude.Test` (499 `[TestMethod]`) | MSTest unit | Covers EntityPMs, service helpers, metadata objects. No UpdateService behaviour assertions. |
| Accounting | `Logitude.Accounting.BL` | `Logitude.FullAccounting.Test`, `Logitude.InvoiceTests` | SpecFlow integration | Lacks fast unit coverage for `ARInvoiceUpdateService`, tax calculators, GL posting. |
| CRM | `Logitude.CRM.BL` | `Logitude.CRMTests` | SpecFlow integration | Heavy reliance on SpecFlow builders; no UpdateService + repository unit tests. |
| Cargo Tracking | `Logitude.CargoTracking.BL` | `Logitude.CargoTrackingTests` | SpecFlow integration | No unit tests around voyage schedules, milestone services. |
| Customs | `Logitude.Customs.BL` | `Logitude.IntegrationTest.Customs` | Integration | No isolated validation/unit suites for `CustomsDeclarationUpdateService`. |
| Dashboard | `Logitude.DashboardModule.BL` | None | — | No automated coverage identified. |
| Booking Library | `Logitude.BookingLib.BL` | None | — | No automated coverage identified. |
| Shipment Order | `Logitude.ShipmentOrderModule.BL` | `Logitude.ShipmentOrderTests` | SpecFlow integration | Missing service-level unit coverage. |
| Shipments | `Logitude.ShipmentModule` (`Logitude.ShipmentTests`) | SpecFlow integration | No unit coverage for routing/workflow validators. |
| Warehouse | `Logitude.WarehouseLib.BL` | None | — | No automated coverage identified. |
| Tariff | `Logitude.TariffModule.BL` | None | — | No automated coverage identified. |
| Time Management | `Logitude.TimeManagement.BL` | `Logitude.TimeManagementTests` | SpecFlow integration | Lacks UpdateService unit coverage. |
| Social | `Logitude.Social.BL` | None | — | No automated coverage identified. |
| Workflow | `Logitude.Workflow.BL` | None | — | No automated coverage identified. |
| Data layer | `Simplog.Data`, `Simplog.Global.Data` | None | — | Repositories currently untested in isolation. |

> **Generated file sources:** module list from `Get-ChildItem 'Logitude.*.BL'`, test suite list from `Get-ChildItem 'Logitude.*Tests'`. Counts obtained via `rg '\[TestMethod\]' Logitude.Test`.

## Gaps vs. Plan

1. **Blind spots** – 9 of 13 BL modules have zero unit-level coverage. Most business rules are only exercised through SpecFlow or integration suites.
2. **UpdateService & Repository behaviour** – No unit tests exercise `ServiceResponse` failure branches, tenant filtering, or composite key logic outside infrastructure tests.
3. **Data layer abstractions** – `Simplog.Data` repositories lack mocks/verification around query filters (`Tenant`, `InActive`, eager loading expectations).
4. **Tooling** – CI cannot fail for low coverage because we do not produce coverage reports today.

## Recommended Coverage Command (when tooling available)

Once `vstest.console.exe` or `dotnet test` with code coverage collectors is available, capture module-level metrics via:

```powershell
# Build (Release) to ensure instrumentation-ready binaries
msbuild Logitude2-5.sln /t:Build /p:Configuration=Release

# Run MSTest unit assemblies with coverage
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" `
  .\Logitude.Test\bin\Release\Logitude.Test.dll `
  /EnableCodeCoverage `
  /Settings:.runsettings `
  /InIsolation

# Convert coverage to Cobertura for CI dashboards
reportgenerator "-reports:TestResults\*\*.coverage" "-targetdir:coverage-report" "-reporttypes:Cobertura"
```

*(Update path/version once VS tooling is installed. For multiple assemblies, append each `*.dll` argument.)*

## Next Actions

- Instrument the build agent with Visual Studio Test Platform or install `Microsoft.TestPlatform` CLI so coverage can be generated automatically.
- Create a reusable `.runsettings` file to exclude generated POCOs and meta-data proxies from coverage calculations.
- Feed the inventory above into the prioritisation grid (Step 2 of the plan) to target high-risk BL modules first.


