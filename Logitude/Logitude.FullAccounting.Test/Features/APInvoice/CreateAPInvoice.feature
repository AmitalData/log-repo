@Pre-Prepare-APIvoice
Feature: Create AP Invoice
	We want to create AP invoice.
@Smoke
Scenario: Create AP Invoice
	Given I have the following AP invoice lines:
		| Description  | VatType | ForiegnCurrencyIdByCode | ForiegnExchangeRate | VatPercentage | InvoiceCurrencyAmount | ChargesType | ProfitCurrencyAmount | LocalCurrencyAmount | ForiegnCurrencyAmount |
		| SpecFlowTest | Zero    | NIS                     | 1                   | 0             | 1                     | ITMS        | 2.12                 | 1                   | 1                     |
	And a AP invoice with the following properties
		| property                    | Value         |
		| Vendor                      | Vendor1       |
		| Branch                      | BerzeitU      |
		| InvoiceCurrency             | NIS           |
		| InvoiceCurrencyExchangeRate | 1             |
		| ProfitCurrency              | NIS           |
		| ProfitCurrencyExchangeRate  | 1             |
		| DueDate                     | Tomorrow      |
		| InvoiceNumber               | Random number |
		| VATNumber                   | 1             |
	When create AP invoice
	Then the AP invoice should create successfully