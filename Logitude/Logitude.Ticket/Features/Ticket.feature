Feature: Create Get Update Ticket
	We want to create, get and update ticket.

Scenario: Create ticket
	Given a ticket with the following properties
		| property    | Value            |
		| EntityType  | Shipment         |
		| Stage       | Open             |
		| Subject     | specflow subject |
		| Description | specflow desc    |
	When create ticket
	Then the ticket should create successfully

Scenario: Get ticket
	When get ticket with TicketId
	Then ticket should be avaliable

Scenario: Update ticket
	Given a ticket
	And following ticket properties
		| property    | Value                    |
		| EntityType  | Quote                    |
		| Subject     | updated specflow subject |
		| Description | updated specflow desc    |
	When update ticket
	Then the ticket should update successfully