Feature: Save Ticket Test

    Scenario: Create new ticket
        Given the user logged in and navigated to ticket workspace
        And a ticket with the following details
            | EntityType | Company   | Contact      | Subject     | Description | MainClassification | Severity | EmployeeGroup | Owner        |
            | shipment   | TestAgent | Test Contact | Test Ticket | Test Ticket | Test               | Medium   | Tester Group  | specflowTest |
        When create ticket
        Then the ticket should create successfully

    Scenario: Save as close the ticket
        Given the user in the ticket's main page
        When save as close
        Then the ticket should save successfully

    Scenario: Save as open the ticket
        When save as open
        Then the ticket should save successfully

    Scenario: Save as resolved the ticket
        When save as resolve
        Then the ticket should save successfully
