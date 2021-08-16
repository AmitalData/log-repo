Feature: Get Tariff Settings access
	With pre-prepared users authentication
	We want to test Tariff settings access.

Scenario: Get tariff settings for user's tenant
	When get tariff settings for user's tenant
	Then tariff settings should be available