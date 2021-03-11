@smoke @release @stable @all
Feature: Actions Ticket test
    This file will create ticket then test the following Actions
    Cancel , Reactivate and Close Without Notifying

    Scenario: Create new ticket
        Given the user logged in and navigated to ticket workspace
        And a ticket with the following details
            | EntityType         | shipment     |
            | Company            | TestAgent    |
            | Contact            | Test Contact |
            | Subject            | Test Ticket  |
            | Description        | Test Ticket  |
            | MainClassification | Test         |
            | Severity           | Medium       |
            | EmployeeGroup      | Tester Group |
            | Owner              | specflowTest |
        When create ticket
        Then the ticket should create successfully

    Scenario: Cancel the ticket
        Given the user in the ticket's main page
        When cancel
        Then the ticket should cancel successfully

    Scenario: Reactivate the ticket
        When reactivate
        Then the ticket should reactivate successfully

    Scenario: Close Without Notifying
        When close without notifying
        Then the ticket should close successfully