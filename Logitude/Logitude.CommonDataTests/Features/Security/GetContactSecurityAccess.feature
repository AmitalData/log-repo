Feature: Get Contact Security Access

Scenario: Get Contact From User's Tenant
	When First user get the first contact from contacts list
	Then the Contact for first user should be exists

Scenario: Get Contact From Other Tenant
	When Second user get the contact that requested by first user
	Then the Contact for second user should not be exists