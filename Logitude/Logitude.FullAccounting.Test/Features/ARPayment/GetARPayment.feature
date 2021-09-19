@Pre-Prepare-GetARPayment
Feature: Get AR Payment
	We want to get ar payment.

Scenario: Get ar Payment
	When get ar payment with ARPaymentId
	Then ar payment should be avaliable