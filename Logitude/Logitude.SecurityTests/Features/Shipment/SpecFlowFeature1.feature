Feature: Just Test Feature

Background:
	Given User email is ahmadb123@mail.com and password is ahmed13!A15
	When User make login request
	Then User should have token

Scenario: Just Test Scenario
	Given User request the shipments list
	When User get the first shipment from shipments list
	Then Shipment should be exists