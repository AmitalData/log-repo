Feature: Get AP Payment Security Access
		With pre-prepared user authentication
		We want to test Get AP Payment Security Access

Scenario: Get AP Payment from user's tenant 
	When get a AP Payment from user's AP Payment list
	Then the AP Payment should exist

Scenario: Get AP Payment from other tenant  
	When get a AP Payment from Other Tenant
	Then the AP Payment should not exist