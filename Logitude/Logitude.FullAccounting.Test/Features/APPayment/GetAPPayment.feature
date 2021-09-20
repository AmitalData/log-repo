@Pre-Prepare-GetAPPayment
Feature: Get AP Payment
	We want to get ap payment.

Scenario: Get ap payment
	When get ap payment with APPaymentId
	Then ap payment should be avaliable