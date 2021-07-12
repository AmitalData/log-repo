@Pre-Prepare-Activity-PhoneCall
Feature: Get Phone call
	The API retrieves phone call.

Scenario: Get phone call
	When get phone call with PhoneCallId
	Then phone call should be avaliable