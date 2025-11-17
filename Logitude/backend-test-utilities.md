# Backend Unit Test Toolkit

## New utilities (2025-11-10)

| Utility | Location | Purpose |
| --- | --- | --- |
| `TenantFixture` | `Logitude.Test/Utilities/TenantFixture.cs` | Tenant-aware defaults (`Tenant=9999`, user/branch IDs), deterministic time handling, reflection helpers to stamp tenant metadata on PM/POCO objects. |
| `ServiceResponseAssertions` | `Logitude.Test/Utilities/ServiceResponseAssertions.cs` | Reflection-based assertions for `HasError`/error-collection patterns; enables consistent validation without binding to a specific response type. |
| `JustMockRegistry` | `Logitude.Test/Utilities/JustMockRegistry.cs` | Lightweight registry to build per-test dependency graphs and manage Telerik JustMock doubles without touching global Unity container state. |

## Usage Guidelines

- **Tenant context**  
  ```csharp
  var invoice = TenantFixture.WithTenant(new APInvoicePM());
  TenantFixture.ArrangeTenantClock(() => DateTime.Parse("2025-01-01T08:00:00Z"));
  ```
  Ensures `Tenant`, `TenantId`, and `BranchId` fields are populated and date-based logic is deterministic.

- **Service response assertions**  
  ```csharp
  var result = service.InsertPM(invoicePm);
  ServiceResponseAssertions.AssertSuccess(result);
  ```
  Works with any response object that exposes `HasError`, `Errors`, `ErrorsArray`, or `ErrorMessages` properties.

- **Mock registry**  
  ```csharp
  var registry = new JustMockRegistry();
  var repository = registry.RegisterMock<IARInvoiceRepository>();
  Mock.Arrange(() => repository.Insert(Arg.IsAny<APInvoice>())).DoNothing();
  var sut = new ARInvoiceService(registry.Resolve<IARInvoiceRepository>(), TenantFixture.DefaultTenantId);
  ```
  Replace ad-hoc dictionaries in future unit tests with the registry to simulate Unity registrations.

## Next Steps

- Extend `JustMockRegistry` with optional factories (lazy instantiation) and scoped disposal.
- Provide base test classes (`AccountingServiceTestBase`, `CustomsServiceTestBase`) that compose `TenantFixture` + `JustMockRegistry` for Tier 1 modules.
- Add FluentAssertions adapters once MSTest + FluentAssertions package strategy is finalized.


