@Pre-Prepare-Activity-PhoneCall
Feature: Update Phone Call
	We want to update phone call.

Scenario: Update phone call
	Given phone call
	And following phone call properties
		| property      | Value                 |
		| Subject       | updated specflow sub  |
		| Description   | updated specflow desc |
		| Duration      | 45                    |
		| DueDate       | 2021-12-15 14:40      |
		| Priority      | Low                   |
	When update phone call
	Then the phone call should update successfully