Feature: Create Shipments
	Create master and house shipments.

#Background:
#	Given Users with following credentials
#		| Email               | Password    |
#		| ahmadb123@mail.com  | ahmed13!A15 |
#	When Users make login request
#	Then Users should have token

Scenario: Create a master shipment
	Given A master shipment fields
		| Direction | TransportMode | ShipmentLevel | FreightPrepaidCollectId | OtherPrepaidCollectId | MainCarriageToPort | MainCarriageFromPort |
		| E         | A             | C             | P                       | C                     | LHR                | MIA                  |
	When Create master shipment API request sent
	Then A new master created successfully

Scenario: Create a house shipment
	Given A house shipment fields
		| NewConcurrencyGUID        | DirectionId | TransportModeId | ShipmentLevelCode | BranchId | DepartmentId | FreightPrepaidCollectId | OtherPrepaidCollectId | CreatedByUserId | UpdatedByUserId | MainCarriageToPortId | MainCarriageFromPortId | CustomerId |
		| Guid.NewGuid().ToString() | E           | A               | H                 | 1-1102   | 1-2988       | P                       | C                     | 1-108265        | 1-108265        | 1-300930             | 1-303023               | 1-200716   |  
	And A master shipment
	When Create house shipment API request sent
	Then A new house created successfully