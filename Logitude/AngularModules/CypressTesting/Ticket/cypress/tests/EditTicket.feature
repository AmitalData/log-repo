@smoke @smoke2 @release @stable @all
Feature: Edit Ticket Test

    Scenario: Create new ticket
        Given the user logged in and navigated to ticket workspace
        And a ticket with the following details
            | EntityType         | shipment                |
            | Company            | TestAgent               |
            | Contact            | TestAgentExport Contact |
            | Subject            | Test Ticket             |
            | Description        | Test Ticket             |
            | MainClassification | Test                    |
            | Severity           | Medium                  |
            | EmployeeGroup      | Tester Group            |
            | Owner              | specflowTest            |
        When create ticket
        Then the ticket should create successfully

    Scenario: Edit the ticket
        Given the user fill the description with "Ticket Test Edited"
        When save as open
        Then the ticket should save successfully
