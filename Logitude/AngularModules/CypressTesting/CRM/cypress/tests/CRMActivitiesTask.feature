@release @stable @smoke @smoke3
Feature: Task Create, Search, and Edit from CRM
    The user creates a task, searches for and edits it from the CRM Module.

    Scenario: Create new task
        Given the user logged in and open Activites in CRM
        And navigate task wizerd and fill the following details
            | Subject      | CurrentDate          |
            | Description  | new description      |
            | PriorityCode | Normal               |
        When create task
        Then the task should create successfully

    Scenario: Search for the task by subject
        When search task
        Then the task should appear successfully

    Scenario: Open the task
        When open task
        Then the task should open successfully

    Scenario: Edit task
        Given add "new note" to main note
        When save task
        Then the task should update successfully

    Scenario: copy task
        Given copy the task with new subject
        And create task
        Then the copy task should create successfully

    Scenario: mark the task as complete
        Given navigate CRM activity screen
        When search task
        And open task
        And press on Mark as Complete button
        Then the task should update successfully
        And the task should appear in My Closed Activites list

    Scenario: reopen task
        When search task
        And open task
        And reopen the task
        Then the task should update successfully
        And the task should appear in My Open Activites list

    Scenario: cancel task
        When search task
        And open task
        And cancel the task
        Then the task should update successfully
        And a red Cancelled label should appear
        And the task should appear in Cancelled Activites list

    Scenario: mark a task as complete from recent activities list
        When press on Complete button
        Then the task should get update