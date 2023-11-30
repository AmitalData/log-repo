@Pre-Prepare-ReturnChequeFromBankDeposit
Feature: Return Cheque From Bank Deposit
	we want to check this api security.

Scenario: Post return cheque bank deposit by not authentication user.
	When post return cheque bank deposit by not authentication user
	Then the post return cheque bank deposit api should return you have no permissions

Scenario: Post return cheque bank deposit by not authorize user.
	When post return cheque bank deposit by not authorize user
	Then the post return cheque bank deposit api should return you have no permissions
