@Pre-Prepare-CrateAPPayment
Feature: Create AP Payment
	We want to create ap payment.

Scenario: Create ap payment
	Given a ap payment with the following properties
		| property                    | Value    |
		| Branch                      | BerzeitU |
		| LocalCurrency               | NIS      |
		| PaymentNo                   | Random   |
		| PaymentCurrency             | NIS      |
		| Vindor                      | Vindor   |
		| AmountInLocalCurrency       | 1        |
		| PaymentCurrencyExchangeRate | 1        |
		| AccountingPaymentMethod     | Cash     |
		| AmountInPaymentCurrency     | 1        |
		| OpenAmount                  | 1        |
		| TaxDeductionLocalAmount     | 1        |
		| TaxDeductionPercentage      | 1        |
	When create ap payment
	Then the ap payment should create successfully