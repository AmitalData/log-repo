Feature: Get FTP Detail Security Access

Scenario: Get FTP Detail From User's Tenant
	When First user get the first FTP Detail from FTP Detail list
	Then the Detail for first user should be exists

Scenario: Get FTP Detail From Other Tenant
	When Second user get the FTP Detail that requested by first user
	Then the Detail for second user should not be exists