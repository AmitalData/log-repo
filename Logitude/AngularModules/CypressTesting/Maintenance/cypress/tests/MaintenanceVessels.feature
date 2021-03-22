@release @all @dev
Feature: Vessel Create, Search and Edit from Maintenance

    The user creates a vessel, searches for and edits it from the Maintenance Module.

    Scenario: Create new vessel
        Given the user logged in and open "Vessels" in maintenance menu
        And a vessel with the following details
            | Name      | Random                  |
            | LocalName | Testing vessel Scenario |
            | IMO       | TVS                     |
            | Flag      | United States           |
            | Notes     | vessel Notes            |
        When create vessel
        Then the vessel should create successfully

    Scenario: Search for the vessel by name
        When search vessel
        Then the vessel should appear successfully

    Scenario: Open the vessel
        When open vessel
        Then the vessel should open successfully

    Scenario: Edit the vessel
        Given the user fill the following vessel details
            | Code | Random |
            | IMO  | TVSS   |
        When edit vessel
        Then the vessel should update successfully

    Scenario: Save and close the vessel
        When save and close vessel
        Then the vessel should close successfully