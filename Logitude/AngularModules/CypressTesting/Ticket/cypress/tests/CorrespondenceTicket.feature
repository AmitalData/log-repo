Feature: Correspondence Ticket Test
    this file will test the Reply and Add Internal Note

    Scenario: Create New Ticket
        Given the user logged in and navigated to ticket workspace
        And a ticket with the following details
            | EntityType | Company   | Contact      | Subject     | Description | MainClassification | Severity | EmployeeGroup | Owner        |
            | shipment   | TestAgent | Test Contact | Test Ticket | Test Ticket | Test               | Medium   | Tester Group  | specflowTest |
        When create ticket
        Then the ticket should create successfully

    Scenario: Reply to the correspondence
        Given the user in the ticket's main page || the user open the ticket
        When reply || reply to the correspondence
        Then the reply should appear successfully || the ticket should update successfully

    Scenario: Add an internal note in the Correspondence
        When add an internal note
        Then the internal note should appear successfully || the ticket should update successfully