Feature: Get AP Payment Security Access

Background:
    Successful login with valid credentials for set of users
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Get AP Payment From User's Tenant
	When First user get the first AP Payment from AP Payments list
	Then AP Payment for first user should be exists

Scenario: Get AP Payment From Other Tenant
	When Second user get the AP Payment that requested by first user
	Then AP Payment for second user should not be exists