@Pre-Prepare-CreateBankAccount
Feature: Create Bank Account
	We want to create bank account.
@Smoke
Scenario: Create bank account
	Given a bank account with the following properties
		| property             | Value             |
		| Bank                 | bank1             |
		| BranchNumber         | 1                 |
		| AccountNumber        | Random            |
		| Currency             | NIS               |
		| GLAccount            | New Account       |
		| DeferredGLAccountId  | New Account       |
		| TransferGLAcccountId | New Account       |
		| Name                 | bank account Test |
	When create bank account
	Then the bank account should create successfully