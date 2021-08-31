@Pre-Prepare
Feature: GetShipmentContainersStatuses
	With pre-prepared FCL shipment data
	We want to get shipment status.

Scenario: Get Shipment Containers Statuses
	Given Read the file "Shipment.xml" shipment data response
	When get Shipment status request
	Then The status of the containers in the shipment should be changed successfully