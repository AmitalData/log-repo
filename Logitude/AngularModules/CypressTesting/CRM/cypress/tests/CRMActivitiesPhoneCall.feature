@stable @all @smoke @smoke3
Feature: Phone Call Create, Search, and Edit from CRM
    The user creates a phone call, searches for and edits it from the CRM Module.

    Scenario: Create new phone call
        Given the user logged in and open Activites in CRM
        And navigate phone call wizerd and fill the following details
            | Customer     | TestCustomer         |
            | CallWith     | TestCustomer Contact |
            | Subject      | CurrentDate          |
            | Description  | new description      |
            | PriorityCode | Normal               |
        When create phone call
        Then the phone call should create successfully

    Scenario: Search for the phone call by subject
        When search phone call
        Then the phone call should appear successfully

    Scenario: Open the phone call
        When open phone call
        Then the phone call should open successfully

    Scenario: Edit phone call
        Given add "new note" to main note
        When save phone call
        Then the phone call should update successfully

    Scenario: copy phone call
        Given copy the phone call with new subject
        And create phone call
        Then the copy phone call should create successfully

    Scenario: mark the phone call as complete
        Given navigate CRM activity screen
        When search phone call
        And open phone call
        And press on Mark as Complete button
        Then the phone call should update successfully
        And the phone call should appear in My Closed Activites list

    Scenario: reopen phone call
        When search phone call
        And open phone call
        And reopen the phone call
        Then the phone call should update successfully
        And the phone call should appear in My Open Activites list

    Scenario: cancel phone call
        When search phone call
        And open phone call
        And cancel the phone call
        Then the phone call should update successfully
        And a red Cancelled label should appear
        And the phone call should appear in Cancelled Activites list

    Scenario: mark a phone call as complete from recent activities list
        When press on Complete button
        Then the phone call should get update