@Pre-Prepare-GetAPIvoice
Feature: Get AP Invoice
	We want to get ap invoice.
@Smoke
Scenario: Get ap invoice
	When get ap invoice with ARInvoiceId
	Then ap invoice should be available