Feature: Cancel and Reactive Ticket Test

    Scenario: Create new ticket
        Given the user logged in and navigated to ticket workspace
        And a ticket with the following details
            | EntityType | Company   | Contact      | Subject     | Description | MainClassification | Severity | EmployeeGroup | Owner        |
            | shipment   | TestAgent | Test Contact | Test Ticket | Test Ticket | Test               | Medium   | Tester Group  | specflowTest |
        When create ticket
        Then the ticket should create successfully

    Scenario: Cancel the ticket
        Given the user in the ticket's main page
        When cancel
        Then the ticket should cancel successfully

    Scenario: Reactivate the ticket
        When reactivate
        Then the ticket should reactivate successfully