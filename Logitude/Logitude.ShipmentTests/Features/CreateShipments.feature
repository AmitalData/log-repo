@Pre-Prepare
Feature: Create Shipments
	With pre-prepared base and shipment data
	We want to create master and house shipments.

Scenario: Create master shipment
	Given a master shipment with the following properties
		| property              | Value   |
		| Direction             | Export  |
		| TransportMode         | Air     |
		| ShipmentLevel         | Master  |
		| FreightPrepaidCollect | Prepaid |
		| OtherPrepaidCollect   | Collect |
		| MainCarriageToPort    | LHR     |
		| MainCarriageFromPort  | MIA     |
	When create master shipment
	Then the master should create successfully

Scenario: Create house shipment 
	Given a house shipment with the following properties
		| property              | Value   |
		| Direction             | Export  |
		| TransportMode         | Air     |
		| ShipmentLevel         | House   |
		| FreightPrepaidCollect | Prepaid |
		| OtherPrepaidCollect   | Collect |
		| MainCarriageToPort    | LHR     |
		| MainCarriageFromPort  | MIA     |
	And a master shipment
	When create house shipment
	Then the house should create successfully