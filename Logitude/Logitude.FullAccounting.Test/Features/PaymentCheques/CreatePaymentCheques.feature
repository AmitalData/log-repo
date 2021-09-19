Feature: Create Payment Cheques
	We want to payment cheques.

Scenario: Create payment cheques
	Given a payment cheques with the following properties
		| property     | Value        |
		| ChequeNumber       | Random   |
		| Currency     | NIS          |
		| BankAccount      | new Bank Account  |
		| LocalAmount | 100         |
		| PayToGLAccount         | new account |
		| PayToName         | Bank Account Test |
	When create payment cheques
	Then the payment cheques should create successfully