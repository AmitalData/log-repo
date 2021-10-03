Feature: PUT Shipment Direct Export Air
	The API updates a Direct Export Air shipment details, adds packages, 
	payables, receivables and creates a new agent as a partner.
@Smoke
@Release 
Scenario: PUT Shipment Direct Export Air
	Given a direct package with the following properties
		| property | Value |
		| Quantity | 200   |
		| Length   | 100   |
		| Width    | 100   |
		| Weight   | 200   |
		| Height   | 100   |
	And  a direct payable with the following properties
		| property                  | Value       |
		| ChargesTypeName           | Air Freight |
		| ChargesType               | AFT         |
		| Measurement               | GRWT        |
		| UnitPrice                 | 60          |
		| Currency                  | EUR         |
		| ShipmentPayableLineStatus | OAMT        |
	And a direct receivable with the following properties
		| property                         | Value       |
		| ChargesTypeName                  | Air Freight |
		| ChargesType                      | AFT         |
		| Measurement                      | GRWT        |
		| Currency                         | EUR         |
		| ShipmentReceivableLineStatusCode | OAMT        |
	And a partners with the following properties
		| property    | Value     |
		| PartnerType | AG        |
		| EnglishName | TestAgent |
		| Country     | US        |
	And a direct shipment
	When update a direct shipment
	Then the direct should update successfully