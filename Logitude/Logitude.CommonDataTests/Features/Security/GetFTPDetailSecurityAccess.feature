Feature: Get FTP Detail Security Access

Background:
    Successful login with valid credentials for set of users
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Get FTP Detail From User's Tenant
	When First user get the first FTP Detail from FTP Detail list
	Then the Detail for first user should be exists

Scenario: Get FTP Detail From Other Tenant
	When Second user get the FTP Detail that requested by first user
	Then the Detail for second user should not be exists