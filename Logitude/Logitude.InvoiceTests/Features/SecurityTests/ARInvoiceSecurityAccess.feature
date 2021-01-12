Feature: AR Invoice Security Access

Background: Login and get a token
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Get AR Invoice for User's Tenant
	When Get AR Invoice request sent for User's Tenant
	Then AR Invoice should be exists

Scenario: Get AR Invoice for other Tenant
	When Get AR Invoice request sent for other Tenant
	Then AR Invoice should not be exists