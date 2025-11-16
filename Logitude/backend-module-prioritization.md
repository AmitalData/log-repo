# Backend Module Prioritisation Grid

_Last updated: 2025-11-10_

## Priority Tiers

- **Tier 1 – Immediate**: High business criticality, high change velocity, no unit coverage. Target ≥70% unit coverage within next release cycle.
- **Tier 2 – Near Term**: Medium/high risk with partial coverage or slower change cadence. Target ≥60% unit coverage within two cycles.
- **Tier 3 – Backlog**: Low risk and low change velocity or mostly static data modules. Target ≥50% or opportunistic coverage.

## Module Overview

| Module | Assembly | Business criticality | Change velocity* | Current automated coverage | Unit coverage gap | Target unit coverage | Priority tier | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Accounting | `Logitude.Accounting.BL` | Very High (financial postings, compliance) | High (monthly regulatory updates) | SpecFlow (integration) | No coverage for UpdateServices, tax/VAT calculators, GL exports | ≥75% focusing on validation, posting, ServiceResponse branches | Tier 1 | Requires mocks for `IARInvoiceRepository`, currency services. |
| Customs | `Logitude.Customs.BL` | Very High (regulatory filings) | Medium-High (country-specific customs changes) | Integration (`Logitude.IntegrationTest.Customs`) | No isolated validation/unit tests | ≥70% covering declaration validation, mapping, message builders | Tier 1 | Add tenant aware repository mocks, message queue fakes. |
| Shipments | `Logitude.ShipmentOrderModule.BL`, `Logitude.ShipmentModule` | High (core operations) | High (frequent workflow tweaks) | SpecFlow | Missing UpdateService/unit coverage | ≥70% on routing, milestone services, calculators | Tier 1 | Prioritise `ShipmentUpdateService`, cost allocation rules. |
| CRM | `Logitude.CRM.BL` | High (sales funnel) | Medium | SpecFlow | Missing unit coverage for opportunity scoring, validation | ≥65% | Tier 2 | Focus on `OpportunityUpdateService`, activities. |
| Warehouse | `Logitude.WarehouseLib.BL` | High (inventory accuracy) | Medium | None | Full gap | ≥65% (pick/pack, allocation, validation) | Tier 2 | Requires builder utilities for location/stock PMs. |
| Tariff | `Logitude.TariffModule.BL` | Medium-High (pricing accuracy) | Medium | None | Full gap | ≥60% (rate calculation, rule evaluation) | Tier 2 | Should reuse tariff scenario fixtures. |
| Infrastructure/Core | `Logitude.Infrastructure.BL`, `Logitude.BL` | High (shared utilities) | Medium | MSTest (499 unit tests) | Missing service orchestration cases | ≥80% (augment existing suite) | Tier 2 | Extend to SessionLocator proxies, ServiceResponse factories. |
| Time Management | `Logitude.TimeManagement.BL` | Medium | Medium | SpecFlow | No unit coverage for scheduling validators | ≥60% | Tier 3 | Secondary; dependent on resource availability. |
| Dashboard | `Logitude.DashboardModule.BL` | Medium | Low | None | Full gap | ≥50% | Tier 3 | Lower risk; focus on query/mapping correctness. |
| Booking Library | `Logitude.BookingLib.BL` | Medium | Low | None | Full gap | ≥50% | Tier 3 | Consider once Tier 1/Tier 2 complete. |
| Social | `Logitude.Social.BL` | Low | Low | None | Full gap | ≥40% | Tier 3 | Minimal business impact. |
| Workflow | `Logitude.Workflow.BL` | Medium | Medium-Low | None | Full gap | ≥55% | Tier 3 | Dependent on BPM changes. |
| Data Layer | `Simplog.Data`, `Simplog.Global.Data` | Cross-cutting | Medium | None | Full gap | ≥60% on repositories/extensions | Tier 2 | Cover tenant filtering, query builders. |

\*Change velocity inferred from historical feature cadence documented in sprint notes and commit frequency for each module (last 6 months).

## Immediate Focus (Tier 1)

1. **Accounting** – Validate financial posting logic, VAT/tax computations, ServiceResponse error flows. Highest audit/regulatory risk.
2. **Customs** – Regulatory compliance requires deterministic validation; failures cause customs rejections.
3. **Shipments** – Frequent operational adjustments; regressions are highly visible to end-users.

## Supporting Data

- Module discovery: `Get-ChildItem -Directory 'Logitude.*.BL'`
- SpecFlow suites: `Get-ChildItem -Directory 'Logitude.*Tests'`, `Logitude.FullAccounting.Test`
- Unit test density: `rg '\[TestMethod\]' Logitude.Test` → 499 hits.

## Next Steps

- Translate Tier 1 priorities into concrete test charters (entry/exit criteria, edge cases).
- Feed this grid into the shared utilities design (Step 3) to ensure builders/mocks cover Tier 1 domains first.
- Revisit priorities quarterly or when roadmap shifts (e.g., Warehouse modernisation project).


