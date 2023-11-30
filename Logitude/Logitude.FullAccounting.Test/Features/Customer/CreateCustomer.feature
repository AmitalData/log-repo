Feature: CreateCustomer
	We want to create customer.

@Smoke
Scenario: Create customer
	Given a customer with the following properties
		| Name |
		| Test |
	When create customer
	Then the customer should create successfully