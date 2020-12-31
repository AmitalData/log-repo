Feature: Get Shipment Security Access

Background:
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Get Shipment From User's Tenant
	Given First user request the shipments list
	When Get the first shipment from shipments list
	Then Shipment should be exists

Scenario: Get Shipment From Other Tenant
	Given Second user request the shipment that requested by first user
	Then Shipment should not be exists