@GetConnectionStrings
Feature: BuildCargoTrackingDatabaseAfterAdjustingOrAddingShipments
	With pre-prepared base and shipment data
	Update a Shipment and Build Cargo Tracking Data


Scenario: Updating a direct shipment gross weight and building cargo tracking
	Given a direct shipment
	When Updating a shipments GrossWeight and building cargo tables 
	Then the GrossWeight of the cargo tracking shipment with the same id will be updated 

Scenario: Creating a new direct shipment and building cargo tracking
	Given a direct shipment
	When building cargo tables 
	Then a cargo tracking shipment with the same id will be created 