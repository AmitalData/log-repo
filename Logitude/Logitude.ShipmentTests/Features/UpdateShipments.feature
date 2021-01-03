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
	When The shipment create API sent
	Then A new master created successfully

Scenario: Create a house shipment
	Given The house shipment fields
		| Direction | Transport Mode | Shipment Type | From Port | To Port | Customer Type | Customer Name |
    	| Export    | Air            | Air           | EZE       | MVD     | Shipper       | Maheera       |
	And houseMasterId is 1
	When The shipment create API sent
	Then A new house created successfully

Scenario: Add a master packages
	Given The master shipment packages fields
		| Pieces | Length | Width | Hight | Weight |
    	| 1      | 100    | 100   | 100   | 200    |
	And shipmentMasterId is 1
	When The shipment update API sent
	Then A new master packages added successfully

Scenario: Add a house packages
	Given The house shipment packages fields
		| Pieces | Length | Width | Hight | Weight |
    	| 1      | 100    | 100   | 100   | 200    |
	And houseMasterId is 1
	When The shipment update API sent
	Then A new house packages added successfully

Scenario: And Payable Charge Type
	Given The Payable Charge Type fields
		| Charge Type | UOM  | Unit Price | Currency |
    	| Air Freight | GRWT | 100        | USD      |
	And shipmentMasterId is 1
	When The shipment update API sent
	Then The payable cherge type added successfully