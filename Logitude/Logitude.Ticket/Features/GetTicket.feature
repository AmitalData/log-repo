@Pre-Prepare-Ticket
Feature: Get Ticket
	We want to get ticket.

Scenario: Get ticket
	When get ticket with TicketId
	Then ticket should be avaliable