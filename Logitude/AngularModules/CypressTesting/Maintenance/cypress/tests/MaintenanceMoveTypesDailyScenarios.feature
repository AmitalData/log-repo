@release @all @dev @daily @l
Feature: Move Type Mock Create and Edit in Maintenance Module
    The user creates a Move Type mock create and edit another one from the Maintenance Module.

    Scenario:Add Move Type Code with lenght more than 3
        Given the user logged in and navigate to "Move Types" in maintenance menu
        When add "1234" as move type code
        Then a validation message with "Code Field must be less than 3" error should appear

    Scenario: Create a new move type
        Given a move type with the following details
            | Code          | Random                           |
            | EnglishName   | Testing Move Type Daily Scenario |
            | LocalName     | Testing Move Type Daily Scenario |
            | TransportMode | Air                              |
        When create move type
        Then the move type should create successfully

    Scenario: Search for the move type by code
        When search for "TSM" move type
        Then the "TSM" move type should appear successfully

    Scenario: Open the move type
        When open move type
        Then the move type should open successfully

    Scenario: Edit the move type
        Given the user fill the following move type general details
            | LocalName | Test edit LocalName move type |
        When update move type
        Then the move type should update successfully
        And the following event should appear in events tab
            | Event             |
            | Move Type Updated |