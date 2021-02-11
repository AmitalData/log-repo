Feature: FTP detail security access
	With pre-prepared users authentication
	We want to test FTP detail security access.

Scenario: Get FTP detail for user's tenant
	When get FTP detail for user's tenant
	Then FTP detail should available

Scenario: Get FTP detail for other tenant
	When get FTP detail for other tenant
	Then FTP detail should not available