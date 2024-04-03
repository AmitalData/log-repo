Feature: Update Tariff settings access 
	With pre-prepared users authentication
	We want to test Update Tariff settings security access.

Scenario: Update tariff settings for user's tenant
	Given tariff settings for user's tenant
	When update tariff settings for user's tenant
	Then tariff settings should update successfully

Scenario: Update tariff settings for other tenant
    Given tariff settings for the user's tenant
	When update tariff settings for other tenant
	Then should receive error message