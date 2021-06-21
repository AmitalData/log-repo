Feature: POST Quote Direct Export Air
	The API creates a Direct Export Air quote.

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