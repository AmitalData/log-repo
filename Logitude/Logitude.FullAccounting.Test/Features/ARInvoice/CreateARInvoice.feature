@Pre-Prepare-Accounting
@Pre-Prepare-NewGLAccount
Feature: Create AR Invoice
	We want to create AR invoice.

Scenario: Create AR invoice
	Given a AR invoice with the following properties
		| property    | Value            |
		
	When create AR invoice
	Then the AR invoice should create successfully