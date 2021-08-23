@Pre-Prepare-ShipmentOrder
Feature: Cancel Shipment Order
	We want to cancel Shipment Order.

Scenario: Cancel shipment order
	Given the shipment order number
	When cancel shipment order
	Then the shipment order should cancel successfully