Feature: Tenant Security Access

Scenario: Get Tenant for User's Tenant
	When Get Tenant request sent for User's Tenant
	Then Tenant should be exists

Scenario: Get Tenant for other Tenant
	When Get Tenant request sent for other Tenant
	Then Tenant should not be exists
