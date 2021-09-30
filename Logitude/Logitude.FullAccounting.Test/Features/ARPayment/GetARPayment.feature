@Pre-Prepare-GetARPayment
Feature: Get AR Payment
	We want to get ar payment.
@Smoke
Scenario: Get ar payment
	When get ar payment with ARPaymentId
	Then ar payment should be available