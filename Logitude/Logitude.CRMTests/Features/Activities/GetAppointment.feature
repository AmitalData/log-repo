@Pre-Prepare-Activity-Appointment
Feature: Get appointment
	The API retrieves appointment.

Scenario: Get appointment
	When get appointment with AppointmentId
	Then appointment should be avaliable