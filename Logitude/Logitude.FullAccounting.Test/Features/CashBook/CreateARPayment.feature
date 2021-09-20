Feature: Create Cashbook
	We want to create cashbook.

Scenario: Create cashbook
	Given a cashbook with the following properties
		| property     | Value        |
		| Branch       | New Branch   |
		| Currency     | NIS          |
		| Account      | new Account  |
		| CashBookType | Cash         |
		| Name         | CashBookTest |
	When create cashbook
	Then the cashbook should create successfully