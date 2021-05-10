@release @all @dev @weekly
Feature: Special Services Type Create and Edit it in Maintenance Module
    The user creates a Special Services Type and edits it from the Maintenance Module.

    Scenario:Add Special Services Type Code with lenght more than 8
        Given the user logged in and navigate to "Special Services Types" in maintenance menu
        When add "123456789" as special services type code
        Then a validation message with "Code Field must be less than 8" error should appear

    Scenario: Create a new special services type
        Given a special services type with the following details
            | Code        | Random                                       |
            | EnglishName | Testing Special Services Type Daily Scenario |
            | LocalName   | Testing Special Services Type Daily Scenario |
        When create special services type
        Then the special services type should create successfully

    Scenario: Search for the special services type
        When search for special services type
        Then the special services type should appear successfully

    Scenario: Open the special services type
        When open special services type
        Then the special services type should open successfully

    Scenario: Edit the special services type
        Given the user fill the following special services type general details
            | LocalName | Test edit LocalName special services type |
        When update special services type
        Then the special services type should update successfully
        And the following event should appear in events tab
            | Event                         |
            | Special Services Type Updated |