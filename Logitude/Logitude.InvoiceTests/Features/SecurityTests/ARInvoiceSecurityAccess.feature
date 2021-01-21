Feature: AR Invoice Security Access

Scenario: Get AR Invoice for User's Tenant
	When Get AR Invoice request sent for User's Tenant
	Then AR Invoice should be exists

Scenario: Get AR Invoice for other Tenant
	When Get AR Invoice request sent for other Tenant
	Then AR Invoice should not be exists