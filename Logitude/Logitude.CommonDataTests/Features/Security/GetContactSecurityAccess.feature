Feature: Get Contact Security Access

Background:
    Successful login with valid credentials for set of users
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Get Contact From User's Tenant
	When First user get the first contact from contacts list
	Then the Contact for first user should be exists

Scenario: Get Contact From Other Tenant
	When Second user get the contact that requested by first user
	Then the Contact for second user should not be exists