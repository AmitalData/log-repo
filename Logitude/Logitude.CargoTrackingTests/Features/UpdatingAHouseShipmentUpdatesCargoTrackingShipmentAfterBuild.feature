Feature: BuildCargoTrackingDatabaseAfterAdjustingAShipment
	With pre-prepared base and shipment data
	Update a Shipment and Build Cargo Tracking Data


Scenario: Updating a house shipment gross weight and building cargo tracking
	Given a master shipment
	And a house shipment
	When Updating a shipments GrossWeight and building cargo tables 
	Then the GrossWeight of the cargo tracking shipment with the same id will be updated 