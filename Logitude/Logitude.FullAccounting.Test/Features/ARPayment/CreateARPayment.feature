@Pre-Prepare-CrateARPayment
Feature: Create AR Payment
	We want to create ar payment.
@Smoke
Scenario: Create ar payment
	Given a ar payment with the following properties
		| property                    | Value            |
		| Branch                      | BerzeitU         |
		| LocalCurrency               | NIS              |
		| PaymentCurrency             | NIS              |
		| BillTo                      | FAC SpecFlowTest |
		| BillToPartnerTypeId         | Customer         |
		| AmountInLocalCurrency       | 1                |
		| SATTransferStatusCode       | Not Transfered   |
		| PaymentCurrencyExchangeRate | 1                |
		| OpenAmountInLocalCurrency   | 1                |
		| AccountingPaymentMethod     | Cash             |
		| AmountInPaymentCurrency     | 1                |
		| OpenAmount                  | 1                |
		| Cashbook                    | CashNIS          |
	When create ar payment
	Then the ar payment should create successfully