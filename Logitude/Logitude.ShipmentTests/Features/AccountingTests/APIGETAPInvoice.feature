Feature: GET AP Invoice
	The API retrieves AP Invoice.

Scenario: GET AP Invoice
	When get APInvoice with APInvoiceNumber
	Then APInvoice should be avaliable