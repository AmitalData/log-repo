Feature: GET AR Invoice
	The API retrieves AR Invoice.

Scenario: GET AR Invoice
	When get ARInvoice with ARInvoiceNumber
	Then ARInvoice should be avaliable