Feature: POST AR Invoice
	The API creates AR Invoice for a vendor.

Scenario: POST AR Invoice
	Given a direct shipment
	And a receivable with the following properties
		| property                         | Value       |
		| ChargesTypeName                  | Air Freight |
		| ChargesType                      | AFT         |
		| Measurement                      | GRWT        |
		| Currency                         | EUR         |
		| Rate                             | 3.8         |
		| Quantity                         | 20          |
		| UnitPrice                        | 5           |
		| ShipmentReceivableLineStatusCode | OAMT        |
	And a receivable receive invoice with the following properties
		| property        | Value        |
		| Customer        | TestCustomer |
		| InvoiceAmount   | 100          |
		| InvoiceCurrency | EUR          |
		| ExchangeRate    | 3.8          |
		| InvoiceDate     | Today        |
		| PaymentTerms    | Cash         |
		| DueDate         | Today        |
		| VatNumber       | zero         |
	And an receivable invoice line with the following properties
		| property        | Value               |
		| ChargesTypeName | Air Freight         |
		| ChargesType     | AFT                 |
		| VatType         | Zero                |
		| VatPrecentage   | 0                   |
		| Amount          | 100                 |
		| Description     | API POST AP Invoice |
		| Currency        | EUR                 |
		| ExchangeRate    | 3.8                 |
		| Quantity        | 20                  |
		| UnitPrice       | 5                   |
	When create ARInvoice
	Then the ARInvoice should create successfully