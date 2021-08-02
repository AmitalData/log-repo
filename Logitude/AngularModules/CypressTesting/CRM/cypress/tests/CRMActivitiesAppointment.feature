@dev
Feature: Appointment Create, Search, and Edit from CRM
    The user creates a appointment, searches for and edits it from the CRM Module.

    Scenario: Create new appointment
        Given the user logged in and open Activites in CRM
        And navigate appointment wizerd and fill the following details
            | Customer     | TestCustomer         |
            | Subject      | CurrentDate          |
            | Description  | new description      |
            | PriorityCode | Normal               |
        When create appointment
        Then the appointment should create successfully

    Scenario: Search for the appointment by subject
        When search appointment
        Then the appointment should appear successfully

    Scenario: Open the appointment
        When open appointment
        Then the appointment should open successfully

    Scenario: Edit appointment
        Given add "new note" to main note
        When save appointment
        Then the appointment should update successfully

    Scenario: copy appointment
        Given copy the appointment with new subject
        And create appointment
        Then the copy appointment should create successfully

    Scenario: mark the appointment as complete
        Given navigate CRM activity screen
        When search appointment
        And open appointment
        And press on Mark as Complete button
        Then the appointment should update successfully
        And the appointment should appear in My Closed Activites list

    Scenario: reopen appointment
        When search appointment
        And open appointment
        And reopen the appointment
        Then the appointment should update successfully
        And the appointment should appear in My Open Activites list

    Scenario: cancel appointment
        When search appointment
        And open appointment
        And cancel the appointment
        Then the appointment should update successfully
        And a red Cancelled label should appear
        And the appointment should appear in Cancelled Activites list

    Scenario: mark a appointment as complete from recent activities list
        When press on Complete button
        Then the appointment should put complete successfully