Feature: Update Local settings access 
	With pre-prepared users authentication
	We want to test Update Local settings security access.

Scenario: Update Local settings for user's tenant
	Given local settings for user's tenant
	When update Local settings for user's tenant
	Then Local settings should update successfully

Scenario: Update Local settings for other tenant
    Given local settings for the user's tenant
	When update Local settings for other tenant
	Then should receive error message