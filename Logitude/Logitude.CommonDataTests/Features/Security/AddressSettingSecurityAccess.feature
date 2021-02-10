Feature: Address setting security access
	With pre-prepared users authentication
	We want to test address security access.

Scenario: Update address settings for user's tenant
	When update address for user's tenant
	Then address should update successfully

Scenario: Update address settings for other tenant
	When update address for other tenant
	Then should receive error message say no permission to do this operation on tenant