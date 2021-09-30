@Pre-Prepare-GetPaymentCheques
Feature: Get Payment cheques
	We want to get payment cheques.
@Smoke
Scenario: Get payment cheques
	When get payment cheques with paymentChequesId
	Then payment cheques should be 