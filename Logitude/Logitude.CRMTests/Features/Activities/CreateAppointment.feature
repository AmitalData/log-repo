Feature: Create Appointment
	We want to create appointment.

Scenario: Create appointment
	Given a appointment with the following properties
		| property             | Value             |
		| Subject              | specflow sub      |
		| Location             | specflow location |
		| Description          | specflow desc     |
		| ActivityTimeTypeCode | BS                |
		| Duration             | 30                |
		| StartDateTime        | 2021-06-15 14:40  |
		| EndDateTime          | 2021-07-15 14:40  |
		| Priority             | Normal            |
	When create appointment
	Then the appointment should create successfully