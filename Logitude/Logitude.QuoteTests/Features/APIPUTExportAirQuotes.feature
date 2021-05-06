Feature: PUT Quote Direct Export Air
	The API updates direct export air quote by adding charges and packages.

Scenario: PUT Shipment Direct Export Air
	Given a package with the following properties
		| property | Value |
		| Quantity | 200   |
		| Length   | 100   |
		| Width    | 100   |
		| Weight   | 200   |
		| Height   | 100   |
	And  a Charge with the following properties
		| property        | Value       |
		| ChargesTypeName | Air Freight |
		| ChargesType     | AFT         |
		| Measurement     | GRWT        |
		| Currency        | EUR         |
		| ExchangeRate    | 1           |
	And a export air quote
	When update a quote
	Then the quote should update successfully