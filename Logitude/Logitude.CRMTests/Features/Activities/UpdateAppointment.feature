@Pre-Prepare-Activity-Appointment
Feature: Update Appointment
	We want to Appointment task.

Scenario: Update Appointment
	Given an appointment
	And following appointment properties
		| property      | Value                     |
		| Subject       | updated specflow sub      |
		| Location      | updated specflow location |
		| Description   | updated specflow desc     |
		| StartDateTime | 2021-09-15 14:40          |
		| EndDateTime   | 2021-12-15 14:40          |		
		| Duration      | 45                        |
		| Priority      | Low                       |
	When update appointment
	Then the appointment should update successfully