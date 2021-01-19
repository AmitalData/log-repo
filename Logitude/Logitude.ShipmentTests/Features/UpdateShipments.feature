Feature: Update Shipments
	With pre-prepared base and shipment data
	Add packages and payables for master and house shipments.

Scenario: Add a master packages
	Given The master shipment packages fields
		| Quantity | Length | Width | Height | Weight |
    	| 1        | 100    | 100   | 100    | 200    |
	And A master shipment
	When The put API sent to add master packages
	Then A new master packages added successfully

Scenario: Add a house packages
	Given The house shipment packages fields
		| Quantity | Length | Width | Height | Weight |
    	| 1        | 100    | 100   | 100    | 200    |
	And A master shipment
	And A house shipment
	When The put API sent to add house packages
	Then A new house packages added successfully

Scenario: And Payable Charge Type
	Given The Payable Charge Type fields
		| ChargesTypeName | ChargesType | Measurement | UnitPrice | Currency | ShipmentPayableLineStatus |
		| Air Freight     | AFT         | GRWT        | 100       | EUR      | OAMT                      |
	And A master shipment
	When The put API sent to add master Payable
	Then The payable cherge type added successfully