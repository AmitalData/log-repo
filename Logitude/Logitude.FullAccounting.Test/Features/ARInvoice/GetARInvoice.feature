@Pre-Prepare-GetARIvoice
Feature: Get AR Invoice
	We want to get ar invoice.

Scenario: Get ar invoice
	When get ar invoice with ARInvoiceId
	Then ar invoice should be Available