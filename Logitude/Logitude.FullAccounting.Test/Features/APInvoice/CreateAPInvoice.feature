@Pre-Prepare-APIvoice
Feature: Create AP Invoice
	We want to create AP invoice.

Scenario: Create AP Invoice
	Given I have the following AP invoice lines:
		| Description  | VatType | VatPercentage |  InvoiceCurrencyAmount | ChargesType | ProfitCurrencyAmount | LocalCurrencyAmount | ForiegnCurrencyAmount |
		| SpecFlowTest | Zero    | 0             |  1                     | ITMS        | 2.12                 | 1                   | 1                     |
	And a AP invoice with the following properties
		| property                    | Value         |
		| Vendor                      | Vendor1       |
		| Branch                      | BZU           |
		| InvoiceCurrency             | NIS           |
		| InvoiceCurrencyExchangeRate | 1             |
		| ProfitCurrency              | NIS           |
		| ProfitCurrencyExchangeRate  | 1             |
		| DueDate                     | Tomorrow      |
		| InvoiceNumber               | Random number |
	When create AP invoice
	Then the AP invoice should create successfully