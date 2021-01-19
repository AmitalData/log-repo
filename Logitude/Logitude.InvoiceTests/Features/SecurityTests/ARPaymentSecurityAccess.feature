Feature: AR Payment Security Access

Background: Login and get a token
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Get AR Payment for User's Tenant
	When Get AR Payment request sent for User's Tenant
	Then AR Payment should be exists

Scenario: Get AR Payment for other Tenant
	When Get AR Payment request sent for other Tenant
	Then AR Payment should not be exists