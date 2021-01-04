Feature: Update Shipments
	Add house shipment, packages and payables

Background:
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
	When Users make login request
	Then Users should have token

Scenario: Create a master shipment
	Given The master shipment fields
		| NewConcurrencyGUID       | DirectionId | TransportModeId | ShipmentLevelCode | Tenant | BranchId | DepartmentId | FreightPrepaidCollectId | OtherPrepaidCollectId | CreatedByUserId | UpdatedByUserId | MainCarriageToPortId | MainCarriageFromPortId |
		| Guid.NewGuid().ToString()| E           | A               | C                 | 951    | 1-1102   | 1-2988       | P                       | C                     | 1-108265        | 1-108265        | 1-300930             |1-303023                |
	When The post API sent to create master shipment
	Then A new master created successfully

Scenario: Create a house shipment
	Given The house shipment fields
		| NewConcurrencyGUID        | DirectionId | TransportModeId | ShipmentLevelCode | BranchId | DepartmentId | FreightPrepaidCollectId | OtherPrepaidCollectId | CreatedByUserId | UpdatedByUserId | MainCarriageToPortId | MainCarriageFromPortId | CustomerId |
		| Guid.NewGuid().ToString() | E           | A               | H                 | 1-1102   | 1-2988       | P                       | C                     | 1-108265        | 1-108265        | 1-300930             | 1-303023               | 1-200716   |  
	And MasterShipmentDataId is 1-1997645 and MasterShipmentNumber is M1304
	When The post API sent to create house shipment
	Then A new house created successfully

Scenario: Add a master packages
	Given The master shipment packages fields
		| Quantity | Length | Width | Hight | Weight |
    	| 1        | 100    | 100   | 100   | 200    |
	And MasterShipmentId is 1-1997648
	When The put API sent to add master packages
	Then A new master packages added successfully

Scenario: Add a house packages
	Given The house shipment packages fields
		| Quantity | Length | Width | Hight | Weight |
    	| 1        | 100    | 100   | 100   | 200    |
	And HouseShipmentId is 1-1997665
	When The put API sent to add house packages
	Then A new house packages added successfully

Scenario: And Payable Charge Type
	Given The Payable Charge Type fields
		| ChargesTypeName | ChargesTypeId | ChargesTypeCode | MeasurementId | MeasurementCode | UnitPrice | CurrencyId | ShipmentPayableLineStatusCode |
		| Air Freight     | 1-32          | AFT             | 1-24          | GRWT            | 100       | 1-7        | OAMT                          |
	And MasterShipmentId is 1-1997648
	When The put API sent to add master Payable
	Then The payable cherge type added successfully