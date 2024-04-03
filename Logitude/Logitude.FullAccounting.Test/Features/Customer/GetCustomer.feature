@Pre-Prepare-GetCustomer
Feature: Get Customer
	We want to get customer.
@Smoke
Scenario: Get customer
	When get customer with customerId
	Then customer should be available