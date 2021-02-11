Feature: AR Invoice Security Access
		With pre-prepared user authentication
		We want to test Get AR Invoice Security Access

Scenario: Get AR Invoice from user's tenant 
	When get a AR Invoice from user's AR Invoices list
	Then the AR Invoice should exist

Scenario: Get AR Invoice from other tenant  
	When get a AR Invoice from Other Tenant
	Then the AR Invoice should not exist