Feature: Create Ticket
	We want to create ticket.
@Smoke
@Release 
Scenario: Create ticket
	Given a ticket with the following properties
		| property    | Value            |
		| EntityType  | Shipment         |
		| Stage       | Open             |
		| Subject     | specflow subject |
		| Description | specflow desc    |
	When create ticket
	Then the ticket should create successfully