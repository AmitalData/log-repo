Feature: AR Payment Security Access
		With pre-prepared user authentication
		We want to test Get AR Payment Security Access

Scenario: Get AR Payment from user's tenant 
	When get a AR Payment from user's AR Payment list
	Then the AR Payment should exist

Scenario: Get AR Payment from other tenant  
	When get a AR Payment from Other Tenant
	Then the AR Payment should not exist