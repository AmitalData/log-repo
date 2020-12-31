Feature: SpecFlowFeature1

Scenario: Just Test Scenario
	Given User email is ahmadb123@mail.com and password is ahmed13!A15
	When User make login request
	Then User should have token
	When Request first shipment from shipments list
	Then The eequested shipment should be exists