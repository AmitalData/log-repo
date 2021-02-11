Feature: Get AP Invoice Security Access 
		With pre-prepared user authentication
		We want to test Get AP Invoice Security Access

Scenario: Get AP Invoice from user's tenant 
	When get a AP Invoice from user's AP Invoices list
	Then the AP Invoice should exist

Scenario: Get AP Invoice from other tenant  
	When get a AP Invoice from Other Tenant
	Then the AP Invoice should not exist