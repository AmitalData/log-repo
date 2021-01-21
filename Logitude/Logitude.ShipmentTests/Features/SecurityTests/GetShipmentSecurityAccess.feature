Feature: Get Shipment Security Access
	With pre-prepared base and shipment data
	We want to test Get Shipment Security Access.

Scenario: Get Shipment From User's Tenant
	When First user get the first shipment from shipments list
	Then Shipment for first user should be exists

Scenario: Get Shipment From Other Tenant
	When Second user get the shipment that requested by first user
	Then Shipment for second user should not be exists