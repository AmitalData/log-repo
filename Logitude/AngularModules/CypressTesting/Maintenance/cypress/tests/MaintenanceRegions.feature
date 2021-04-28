@release @dev @all 
Feature: Region Create, Edit and Inactivate in Maintenance Module
    The user creates a region, edits and inactivates it from the Maintenance Module.

    Scenario: Add region
        Given the user logged in and navigate to "Regions" in maintenance menu
        And a region with the following details
            | RegionName      | random |
            | RegionLocalName | random |
            | InactiveRegion  | No     |
        When add region
        Then the region should add successfully

    Scenario: Search for the region by name
        When search for region
        Then the region should appear successfully

    Scenario: Open the region
        When open region
        Then the region should open successfully

    Scenario: Edit the region
        Given a "newRegionLocalName" as regionLocalName
        And  the user inactivate region
        When edit region
        Then the region should update successfully
        And following event should appear in events tab
            | Event          | Notes              |
            | Region Updated | Region Inactivated |
