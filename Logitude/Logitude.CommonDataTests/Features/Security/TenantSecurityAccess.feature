Feature: Tenant security access
	With pre-prepared users authentication
	We want to test tenant security access.

Scenario: Get information for user's tenant
	When get information for user's tenant
	Then tenant information should available

Scenario: Get information for other tenant
	When get information for other tenant
	Then should receive error message say not authenticated to view company info