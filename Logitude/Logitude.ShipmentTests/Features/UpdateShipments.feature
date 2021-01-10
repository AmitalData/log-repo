Feature: Update Shipments
	Add packages and payables for master and house shipments.

Background:
	Given Users with following credentials
		| Email               | Password    |
		| ahmadb123@mail.com  | ahmed13!A15 |
	When Users make login request
	Then Users should have token

Scenario: Add a master packages
	Given The master shipment packages fields
		| Quantity | Length | Width | Hight | Weight |
    	| 1        | 100    | 100   | 100   | 200    |
	And A master shipment
	When The put API sent to add master packages
	Then A new master packages added successfully

Scenario: Add a house packages
	Given The house shipment packages fields
		| Quantity | Length | Width | Hight | Weight |
    	| 1        | 100    | 100   | 100   | 200    |
	And A house shipment
	When The put API sent to add house packages
	Then A new house packages added successfully

Scenario: And Payable Charge Type
	Given The Payable Charge Type fields
		| ChargesTypeName | ChargesTypeId | ChargesTypeCode | MeasurementId | MeasurementCode | UnitPrice | CurrencyId | ShipmentPayableLineStatusCode |
		| Air Freight     | 1-32          | AFT             | 1-24          | GRWT            | 100       | 1-7        | OAMT                          |
	And A master shipment
	When The put API sent to add master Payable
	Then The payable cherge type added successfully