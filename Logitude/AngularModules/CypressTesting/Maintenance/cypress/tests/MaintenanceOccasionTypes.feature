@dev
Feature: Occasion Types Create, Search and Edit from Maintenance
    The user creates a occasion type, searches for and edits it from the Maintenance Module.

    Scenario: Create new Occasion Type
        Given the user logged in and open "Occasion Types" in maintenance menu
        And a occasion type with the following details
            | Name | CurrentDatetime |
            | Code | Random          |
        When create occasion type
        Then the occasion type should create successfully

    Scenario: Search for the Occasion Type by code
        When search occasion type
        Then the occasion type should appear successfully

    Scenario: Open the Occasion Type
        When open occasion type
        Then the occasion type should open successfully

    Scenario: Edit the Occasion Type
        Given the user edit the following occasion type details
            | Name | CurrentDatetime |
        When save occasion type
        Then the occasion type should update successfully
        And the following event should appear in events tab
            | Event   | Notes |
            | Updated |       |

    Scenario: Save and close the Occasion Type
        When save and close occasion type
        Then the occasion type should close successfully