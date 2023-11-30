@Pre-Prepare-GetSingleGLAccountByDisplayNumberAndTenant
Feature: Get Single GLAccount By Display Number And Tenant
	We want to get bank single gl account by display number And tenant from unauthorizes tenant.

Scenario: Get single gl account by display number And tenant from unauthorizes tenant.
	When get  single gl account
	Then The get  single gl account by display number And tenant API should return you have no permissions