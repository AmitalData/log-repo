Feature: Get shipment security access
	With pre-prepared base and shipment data
	We want to test Get Shipment Security Access.

Scenario: Get shipment from user's tenant
	When get a shipment from User's shipment list
	Then the shipment should exist

Scenario: Get shipment from other tenant
	When get a shipment from Other Tenant
	Then the shipment should not exist