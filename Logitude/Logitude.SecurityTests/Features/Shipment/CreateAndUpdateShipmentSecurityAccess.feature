Feature: Create and Update Shipment Security Access

Background:
	Successful login with valid credentials for set of users
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
		| protractor@test.com | !P123t456   |
	When Users make login request
	Then Users should have token

Scenario: Create Shipment for User's Tenant
	When Create shipment request sent for User's Tenant
	Then Shipment should be added successfully

Scenario: Create Shipment for other Tenant
	When Create shipment request sent for other Tenant
	Then Shipment should not be added

Scenario: Update Shipment for User's Tenant
	When Update shipment request sent for User's Tenant
	Then Shipment should be Updated successfully

Scenario: Update Shipment for other Tenant
	When Update shipment request sent for other Tenant
	Then Shipment should not be Updated