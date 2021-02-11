Feature: Contact security access
	With pre-prepared users authentication
	We want to test contact security access.

Scenario: Get contact for user's tenant
	When get contact for user's tenant
	Then contact should available

Scenario: Get contact for other tenant
	When get contact for other tenant
	Then contact should not available