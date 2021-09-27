@Pre-Prepare-GetARPayment
Feature: Get AR Payment
	We want to get ar payment.

Scenario: Get ar payment
	When get ar payment with ARPaymentId
	Then ar payment should be Available