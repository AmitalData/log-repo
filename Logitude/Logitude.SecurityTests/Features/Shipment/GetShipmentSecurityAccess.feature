Feature: Get Shipment Security Access

Scenario: Get Shipment From User's Tenant
	Given User email is ahmadb123@mail.com and password is ahmed13!A15
	When User make login request
	Then User should have token
	When Get the first shipment from shipments list
	Then Shipment should be exists

Scenario: Get Shipment From Other Tenant
	Given User email is ahmadb123@mail.com and password is ahmed13!A15
	When User make login request
	Then User should have token
	When Get the first shipment from shipments list
	Then Shipment should be exists
	Given User email is protractor@test.com and password is !P123t456
	When User make login request
	Then User should have token
	When Get shipment from other tenant
	Then Shipment from other tenant should not be exists