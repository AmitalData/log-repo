Feature: Warmup Shipments
	With pre-prepared base and shipment data
	We want to Get master shipment, then create master and house shipments.

Scenario: Get master shipment
	Given a master shipment 
	When get master shipment
	Then the master should exist

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