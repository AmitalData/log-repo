@smoke @release @all @stable
Feature: Activities Ticket Test
    This file will create ticket then test the following
    Create and complete Call, Task and Appointment activities

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

    Scenario: Create Call Activity
        Given the user in the ticket's main page
        And a subject as "Create Call Activity Test"
        When create phone call activity
        Then the call activity should appear successfully

    Scenario: Create Task Activity
        Given a subject as "Create Task Activity Test"
        When create task activity
        Then the task activity should appear successfully

    Scenario: Create Appointment Activity
        Given a subject as "Create Appointment Activity Test"
        When create appointment activity
        Then the appointment activity should appear successfully

    Scenario: Complete Call Activity
        When complete phone call activity
        Then the call activity should complete successfully

    Scenario: Complete Task Activity
        When complete task activity
        Then the task activity should complete successfully

    Scenario: CreCompleteate Appointment Activity
        When complete appointment activity
        Then the appointment activity should complete successfully