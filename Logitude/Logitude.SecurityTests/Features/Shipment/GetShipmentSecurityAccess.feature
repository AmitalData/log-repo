Feature: Get Shipment Security Access

Background:
    Successful login with valid credentials for set of users
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Get Shipment From User's Tenant
	When First user get the first shipment from shipments list
	Then Shipment for first user should be exists

Scenario: Get Shipment From Other Tenant
	When Second user get the shipment that requested by first user
	Then Shipment for second user should not be exists