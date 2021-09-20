@Pre-Prepare-ARIvoice
Feature: Create AR Invoice
	We want to create AR invoice.

Scenario: Create AR invoice
	Given I have the following AR invoice lines:
		| Line | InvoiceCurrency | ForiegnCurrency | ForiegnExchangeRate | Description  | VatType | VatPercentage | DueDate    | Quantity | InvoiceCurrencyAmount | UnitPrice | ChargesType | ProfitCurrencyAmount | LocalCurrencyAmount | ForiegnCurrencyAmount |
		| 1    | NIS             | NIS             | 1                   | SpecFlowTest | Zero    | 0             | 09/01/2021 | 1        | 1                     | 1         | ITMS        | 2.12                 | 1                   | 1                     |
	And a AR invoice with the following properties
		| property                    | Value    |
		| Branch                      | BerzeitU |
		| InvoiceCurrency             | NIS      |
		| InvoiceCurrencyExchangeRate | 1        |
		| ProfitCurrency              | NIS      |
		| VatNumber                   | 1        |
		| ProfitCurrencyExchangeRate  | 1        |
		| DueDate                     | Tomorrow |
	When create AR invoice
	Then the AR invoice should create successfully