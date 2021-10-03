@Pre-Prepare-GetBankDeposit
Feature: Get Bank Deposit Without Lines 
	We want to get bank deposit without lines from unauthorizes tenant.

Scenario: Get bank deposit without lines from unauthorizes tenant.
	When get bank deposit without lines from unauthorizes tenant
	Then The bank deposit without lines API should return you have no permissions