@Pre-Prepare-CreateBankDeposit
Feature: Create Bank Deposit
	We want to bank deposit.

Scenario: Create bank deposit
	Given a bank deposit with the following properties
		| property            | Value         |
		| CashBook            | new cash book |
		| DepositCurrency     | NIS           |
		| ForeignAmount       | 300           |
		| LocalDepositAmount | 300           |
	When create bank deposit
	Then the bank deposit should create successfully