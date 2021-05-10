Feature: POST AR Invoice
	The API creates AR Invoice for a vendor.

Scenario: POST AR Invoice
	Given a direct shipment
	And a direct receivable with the following properties
		| property                         | Value       |
		| ChargesTypeName                  | Air Freight |
		| ChargesType                      | AFT         |
		| Measurement                      | GRWT        |
		| Currency                         | EUR         |
		| ShipmentReceivableLineStatusCode | OAMT        |
	And a receivable receive invoice with the following properties
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
	And an receivable invoice line with the following properties
		| property        | Value               |
		| ChargesTypeName | Air Freight         |
		| ChargesType     | AFT                 |
		| VatType         | Zero                |
		| VatPrecentage   | 0                   |
		| Amount          | 100                 |
		| Description     | API POST AP Invoice |
	When update a direct shipment
	Then the direct should update successfully