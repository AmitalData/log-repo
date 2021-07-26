@Pre-Prepare-Ticket
Feature: Update Ticket
	We want to update ticket.

Scenario: Update ticket
	Given a ticket
	And following ticket properties
		| property    | Value                    |
		| EntityType  | Quote                    |
		| Subject     | updated specflow subject |
		| Description | updated specflow desc    |
	When update ticket
	Then the ticket should update successfully