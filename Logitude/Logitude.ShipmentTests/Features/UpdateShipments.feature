Feature: Update Shipments
	With pre-prepared base and shipment data
	Add packages and payables for master and house shipments.

Scenario: Add a master package
	Given a master package with the following properties
		| property | Value |
		| Quantity | 1     |
		| Length   | 100   |
		| Width    | 100   |
		| Height   | 100   |
		| Weight   | 200   |
	And a master shipment
	When add a master package
	Then the master should add package successfully

Scenario: Add a house packages
	Given a house package with the following properties
		| property | Value |
		| Quantity | 1     |
		| Length   | 100   |
		| Width    | 100   |
		| Height   | 100   |
		| Weight   | 200   |
	And a master shipment
	And a house shipment
	When add a house package
	Then the house should add package successfully

Scenario: Add a master payable
	Given a master payable with the following properties
		| property                  | Value       |
		| ChargesTypeName           | Air Freight |
		| ChargesType               | AFT         |
		| Measurement               | GRWT        |
		| UnitPrice                 | 100         |
		| Currency                  | EUR         |
		| ShipmentPayableLineStatus | OAMT        |
	And a master shipment
	When add a master payable
	Then the master should add payable successfully