Feature: Quote Controller API's
	The API creates a Direct Export Air quote , then update the quote
	The API get Quote by single , single list and by filter

Scenario: POST Quote Direct Export Air
	Given a quote with the following properties
		| property      | Value     |
		| Direction     | Export    |
		| TransportMode | Air       |
		| QuoteType     | Spot Rate |
		| Currency      | EUR       |
		| ExchangeRate  | 1         |
		| CustomerType  | Shipper   |
		| ToPort        | JFK       |
		| FromPort      | MIA       |
	When create quote
	Then the quote should create successfully

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
	And an export air quote
	When update a quote
	Then the quote should update successfully

Scenario: GET Single Quote Export Air
	When get quote with QuoteId
	Then quote should be avaliable

Scenario: GET Single List Quote Export Air
	When get single list quote with QuoteId
	Then quote should be avaliable