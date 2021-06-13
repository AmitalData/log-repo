@NewDev @weekly
Feature: Department Create, Search and Edit from Maintenance
    The user creates a department, searches for and edits it from the Maintenance Module.

    Scenario: Add Department Code with lenght more than 10
        Given the user logged in and open "Departments" in maintenance menu
        When add "01234567891" as department code
        Then a validation message with "Code Field must be less than 10" error should appear

    Scenario: Create new Department
        Given a department with the following details
            | Name      | CurrentDatetime |
            | LocalName | Local Name      |
            | Code      | Random          |
            | Notes     | Department      |
        When create department
        Then the department should create successfully

    Scenario: Search for the Department by code
        When search department
        Then the department should appear successfully

    Scenario: Open the Department
        When open department
        Then the department should open successfully

    Scenario: Edit the Department
        Given the user edit the following department details
            | LocalName        | New LocalName  |
            | Notes            | New Department |
            | InActiveCheckBox | Yes            |
        When save department
        And the following event should appear in events tab
            | Event              | Notes                  |
            | Department Updated | Department Inactivated |
        Then the department should update successfully

    Scenario: Save and close the Department
        When save and close department
        Then the department should close successfully