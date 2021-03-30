@release @stable @all
Feature: Create Global Zone, Inactivate and activate it from Maintenance
    The user creates a Global Zone, Inactivates it,
    selects it to edit and activates it from the Maintenance module.

    Scenario: Add GlobalZoneCode with lenght more than 8
        Given the user logged in and navigate to "Global Zones" in maintenance menu
        When add "123456789" as Global Zone code
        Then a validation message with "Code Field must be less than 8" error should appear

    Scenario: Add global zone
        Given a global zone with the following details
            | GlobalZoneCode      | random |
            | GlobalZoneName      | random |
            | GlobalZoneLocalName | random |
            | InactiveGlobalZone  | Yes    |
        When add global zone
        Then the global zone should add successfully

    Scenario: Search for the global zone by name
        When search for global zone
        Then the global zone should appear successfully

    Scenario: Open the global zone
        When open global zone
        Then the global zone should open successfully

    Scenario: Edit the global zone
        Given a "random" as globalZoneLocalName
        And  the user activate global zone
        When edit global zone
        Then the global zone should update successfully
        And following event should appear in events tab
            | Event               | Notes                 |
            | Global Zone Updated | Global Zone Activated |
