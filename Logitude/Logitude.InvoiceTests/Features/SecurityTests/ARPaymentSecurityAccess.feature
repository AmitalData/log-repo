Feature: AR Payment Security Access

Scenario: Get AR Payment for User's Tenant
	When Get AR Payment request sent for User's Tenant
	Then AR Payment should be exists

Scenario: Get AR Payment for other Tenant
	When Get AR Payment request sent for other Tenant
	Then AR Payment should not be exists