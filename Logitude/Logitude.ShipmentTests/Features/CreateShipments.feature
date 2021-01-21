Feature: Create Shipments
	With pre-prepared base and shipment data
	We want to create master and house shipments.

Scenario: Create a master shipment
	Given A master shipment fields
		| Direction | TransportMode | ShipmentLevel | FreightPrepaidCollect | OtherPrepaidCollect | MainCarriageToPort | MainCarriageFromPort |
		| E         | A             | C             | P                     | C                   | LHR                | MIA                  |
	When Create master shipment API request sent
	Then A new master created successfully

Scenario: Create a house shipment
	Given A house shipment fields
		| Direction | TransportMode | ShipmentLevel | FreightPrepaidCollect | OtherPrepaidCollect | MainCarriageToPort | MainCarriageFromPort |
		| E         | A             | H             | P                     | C                   | LHR                | MIA                  | 
	And A master shipment
	When Create house shipment API request sent
	Then A new house created successfully