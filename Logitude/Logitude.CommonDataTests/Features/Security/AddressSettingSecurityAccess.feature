Feature: Address Setting Security Access

Scenario: Update Address Settings for User's Tenant
	When Update Address Settings request sent for User's Tenant
	Then Address Settings should be Updated successfully

Scenario: Update Address Settings for other Tenant
	When Update Address Settings request sent for other Tenant
	Then Address Settings should not be Updated