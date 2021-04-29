@smoke @release @all
Feature: Correspondence Ticket Test
    This file will test the Reply and Add Internal Note

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

    Scenario: Reply to the correspondence
        Given the user in the ticket's main page
        When reply to the correspondence
        Then the reply should appear successfully

    Scenario: Add an internal note in the Correspondence
        When add an internal note
        Then the internal note should appear successfully