Feature: Local settings access 
	With pre-prepared users authentication
	We want to test local settings access.

Scenario: Get local settings for user's tenant
	When get local settings for user's tenant
	Then local settings should available

Scenario: Get local settings for other tenant
	When get local settings for other tenant
	Then local settings should not available