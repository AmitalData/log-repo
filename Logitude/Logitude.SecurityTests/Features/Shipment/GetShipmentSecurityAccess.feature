Feature: Get Shipment Security Access

Background:
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Get Users Shipment Depending On Their Tenants
	Given First user request the shipments list
	When First user get the first shipment from shipments list
	*    Second user request the shipment that requested by first user
	Then Users should not be on same tenant
	*    Shipment for first user should be exists
	*    Shipment for second user should not be exists