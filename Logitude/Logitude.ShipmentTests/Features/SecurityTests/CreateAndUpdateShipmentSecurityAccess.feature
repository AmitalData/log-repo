Feature: Create and update shipment security access
	With pre-prepared base and shipment data
	We want to test Create and update shipment security access.

Scenario: Create shipment for user's tenant
	When create a shipment for user's tenant
	Then the shipment should create successfully

Scenario: Create shipment for other tenant
	When create a shipment for other tenant
	Then the shipment should not create successfully

Scenario: Update shipment for user's tenant
	When update a shipment for user's tenant
	Then the shipment should update successfully

Scenario: Update shipment for other tenant
	When update a shipment for other tenant
	Then the shipment should not update successfully