Feature: PUT AR Invoice
	The API updates AR Invoice.

Scenario: PUT AR Invoice
	Given a direct shipment
	And a ARInvoice receivable with the following properties
		| property                         | Value       |
		| ChargesTypeName                  | Air Freight |
		| ChargesType                      | AFT         |
		| Measurement                      | GRWT        |
		| Currency                         | EUR         |
		| Rate                             | 3.8         |
		| Quantity                         | 20          |
		| UnitPrice                        | 5           |
		| ShipmentReceivableLineStatusCode | OAMT        |
	And a ARInvoice receivable receive invoice with the following properties
		| property        | Value        |
		| Customer        | TestCustomer |
		| InvoiceAmount   | 100          |
		| InvoiceCurrency | EUR          |
		| ExchangeRate    | 3.8          |
		| InvoiceDate     | Today        |
		| PaymentTerms    | Cash         |
		| DueDate         | Today        |
		| VatNumber       | zero         |
	And an ARInvoice receivable invoice line with the following properties
		| property        | Value               |
		| ChargesTypeName | Air Freight         |
		| ChargesType     | AFT                 |
		| VatType         | Zero                |
		| VatPrecentage   | 0                   |
		| Amount          | 100                 |
		| Description     | ARI POST AR Invoice |
		| Currency        | EUR                 |
		| ExchangeRate    | 3.8                 |
		| Quantity        | 20                  |
		| UnitPrice       | 5                   |
	When update ARInvoice by edit invoice line with the following properties
		| property        | Value               |
		| ChargesTypeName | Air Freight         |
		| Quantity        | 40                  |
		| Description     | ARI POST AR Invoice |
	Then the ARInvoice should update successfully