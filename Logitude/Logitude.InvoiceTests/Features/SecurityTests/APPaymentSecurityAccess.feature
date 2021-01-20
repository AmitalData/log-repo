Feature: Get AP Payment Security Access

Scenario: Get AP Payment From User's Tenant
	When First user get the first AP Payment from AP Payments list
	Then AP Payment for first user should be exists

Scenario: Get AP Payment From Other Tenant
	When Second user get the AP Payment that requested by first user
	Then AP Payment for second user should not be exists