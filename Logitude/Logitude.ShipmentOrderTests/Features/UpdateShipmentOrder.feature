@Pre-Prepare-ShipmentOrder
Feature: Update Shipment Order
	We want to update Shipment Order.

Scenario: Update shipment order
	Given a shipment order
	And following shipment order properties
		| property           | Value                       |
		| CustomerReferences | updated specflow references |
		| DescriptionOfGoods | updated specflow desc       |
	When update shipment order
	Then the shipment order should update successfully