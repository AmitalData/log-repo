@smoke @Release
Feature: Activities Ticket Test
    this file will create ticket then test the following activities
    create Call, Task and Appointment

    Scenario: Create new ticket
        Given the user logged in and navigated to ticket workspace
        And a ticket with the following details
            | EntityType | Company   | Contact      | Subject     | Description | MainClassification | Severity | EmployeeGroup | Owner        |
            | shipment   | TestAgent | Test Contact | Test Ticket | Test Ticket | Test               | Medium   | Tester Group  | specflowTest |
        When create ticket
        Then the ticket should create successfully

    Scenario: Create Call Activity
        Given the user in the ticket's main page
        When create phone call activity
        Then the call activity should appear successfully

    Scenario: Create Task Activity
        When create task activity
        Then the task activity should appear successfully

    Scenario: Create Appointment Activity
        When create appointment activity
        Then the appointment activity should appear successfully

    Scenario: Complete Call Activity
        When complete phone call activity
        Then the call activity should complete successfully

    # Scenario: Complete Task Activity
    #     When complete task activity
    #     Then the task activity should complete successfully

    # Scenario: CreCompleteate Appointment Activity
    #     When complete appointment activity
    #     Then the appointment activity should complete successfully


