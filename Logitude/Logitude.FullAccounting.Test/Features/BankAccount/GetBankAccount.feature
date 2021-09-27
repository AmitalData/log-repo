@Pre-Prepare-GetBankAccount
Feature: Get Bank Account
	We want to get Bank Account.

Scenario: Get bank account
	When get bank account with bankAccountId
	Then bank account should be available