@smoke @release @all
Feature: Create Ticket Test

    Scenario: Create New Ticket
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
