Feature: Address Setting Security Access

Background: Login and get a token
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Get Address Settings for User's Tenant
	When Get Address Settings request sent for User's Tenant
	Then Address Settings should be exists

Scenario: Get Address Settings for other Tenant
	When Get Address Settings request sent for other Tenant
	Then Address Settings should not be exists

Scenario: Update Address Settings for User's Tenant
	When Update Address Settings request sent for User's Tenant
	Then Address Settings should be Updated successfully

Scenario: Update Address Settings for other Tenant
	When Update Address Settings request sent for other Tenant
	Then Address Settings should not be Updated