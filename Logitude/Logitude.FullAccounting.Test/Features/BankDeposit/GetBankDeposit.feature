@Pre-Prepare-GetBankDeposit
Feature: Get Bank Deposit
	We want to get bank deposit.

Scenario: Get bank deposit
	When get bank deposit with bankDepositId
	Then bank deposit should be Available