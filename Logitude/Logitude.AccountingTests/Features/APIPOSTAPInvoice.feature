Feature: POST AP Invoice
	The API creates AP Invoice for a vendor.

Scenario: POST AP invoice
	Given a direct shipment
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
	When create invoice
	Then the invoice should create successfully