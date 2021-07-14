Feature: PUT AP Invoice
	The API updates AP Invoice.

Scenario: PUT AP Invoice
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
	And a payable receive invoice with the following properties
		| property        | Value       |
		| Vendor          | TestVendor  |
		| InvoiceNumber   | 98675625870 |
		| InvoiceAmount   | 100         |
		| InvoiceCurrency | EUR         |
		| ExchangeRate    | 3.8         |
		| InvoiceDate     | Today       |
		| PaymentTerms    | Cash        |
		| DueDate         | Today       |
		| VatNumber       | zero        |
	And an invoice line with the following properties
		| property        | Value               |
		| ChargesTypeName | Air Freight         |
		| ChargesType     | AFT                 |
		| VatType         | Zero                |
		| VatPrecentage   | 0                   |
		| Amount          | 100                 |
		| Description     | API POST AP Invoice |
	When update APInvoice by adding invoice line with the following properties
		| property        | Value              |
		| ChargesTypeName | Air Freight        |
		| ChargesType     | AFT                |
		| VatType         | Zero               |
		| VatPrecentage   | 0                  |
		| Amount          | 50                 |
		| Description     | API PUT AP Invoice |
	Then the APInvoice should update successfully