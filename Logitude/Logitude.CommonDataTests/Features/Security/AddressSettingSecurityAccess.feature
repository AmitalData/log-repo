Feature: Address Setting Security Access

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