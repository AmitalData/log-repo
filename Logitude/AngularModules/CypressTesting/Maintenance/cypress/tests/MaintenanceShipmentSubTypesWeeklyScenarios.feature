@release @all @dev @weekly 
Feature: Shipment Sub Type Create and Edit it in Maintenance Module
    The user creates a Shipment Sub Type and edits it from the Maintenance Module.

    Scenario:Add Shipment Sub Type Code with lenght more than 6
        Given the user logged in and navigate to "Shipment Sub Types" in maintenance menu
        When add "23456" as shipment sub type code
        Then a validation message with "Code Field must be less than 5" error should appear

    Scenario: Create a new shipment sub type
        Given a shipment sub type with the following details
            | Code          | Random                           |
            | EnglishName   | Testing Shipment Sub Type Daily Scenario |
            | LocalName     | Testing Shipment Sub Type Daily Scenario |
            | TransportMode | Air                              |
        When create shipment sub type
        Then the shipment sub type should create successfully

    Scenario: Search for the shipment sub type
        When search for shipment sub type
        Then the shipment sub type should appear successfully

    Scenario: Open the shipment sub type
        When open shipment sub type
        Then the shipment sub type should open successfully

    Scenario: Edit the shipment sub type
        Given the user fill the following shipment sub type general details
            | LocalName | Test edit LocalName shipment sub type |
        When update shipment sub type
        Then the shipment sub type should update successfully
        And the following event should appear in events tab
            | Event             |
            | Updated |