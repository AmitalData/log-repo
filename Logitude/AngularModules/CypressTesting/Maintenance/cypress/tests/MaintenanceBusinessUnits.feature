@stable @release
Feature: Business Unit Create, Search and Edit from Maintenance
    The user creates a business Unit, searches for and edits it from the Maintenance Module.

    Scenario: Create new Business Unit
        Given the user logged in and open "Business Units" in maintenance menu
        And a business unit with the following details
            | Name   | CurrentTimeDate |
            | Parent | Organization    |
        When create business unit
        Then the business unit should create successfully

    Scenario: Create another business unit
        Given create new business unit
            | Name | CurrentTimeDate |
        When create business unit
        Then the business unit should create successfully

    Scenario: Search for the Business Unit by name
        When search business unit
        Then the business unit should appear successfully

    Scenario: Open the business Unit
        When open business unit
        Then the business unit should open successfully

    Scenario: Edit the Business Unit
        Given the user edit the business unit and the parent should be dim
        When save business unit
        Then the business unit should update successfully
        And the following event should appear in events tab
            | Event                 | Notes                     |
            | Business Unit Updated | Business Unit Inactivated |

    Scenario: Save and close the Business Unit
        When save and close business unit
        Then the business unit should close successfully