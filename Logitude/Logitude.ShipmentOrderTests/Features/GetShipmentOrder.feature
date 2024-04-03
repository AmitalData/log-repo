@Pre-Prepare-ShipmentOrder
Feature: Get Shipment Order
	We want to get Shipment Order.

Scenario: Get shipment order
	When get shipment order with OrderNumber
	Then shipment order should be avaliable