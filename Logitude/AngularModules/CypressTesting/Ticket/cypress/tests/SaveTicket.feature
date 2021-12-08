@smoke @smoke2 @release @stable
Feature: Save Ticket Test
    This file will create ticket then test the following
    Save as close , as open and as resolved

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

    Scenario: Save as close the ticket
        Given the user in the ticket's main page
        When save as close
        Then the "Closed" ticket should save successfully

    Scenario: Save as open the ticket
        When save as open
        Then the "Open" ticket should save successfully

    Scenario: Save as resolved the ticket
        When save as resolve
        Then the "Resolved" ticket should save successfully