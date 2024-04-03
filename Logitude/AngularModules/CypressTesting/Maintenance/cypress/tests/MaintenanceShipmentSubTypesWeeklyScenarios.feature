@release @all @stable @weekly
Feature: Shipment Sub Type Create and Edit it in Maintenance Module
    The user creates a Shipment Sub Type and edits it from the Maintenance Module.

    Scenario:Add Shipment Sub Type Code with lenght more than 6
        Given the user logged in and navigate to "Shipment Sub Types" in maintenance menu
        When add "123456" as shipment sub type code
        Then a validation message with "Code Field length must be less than 5" error should appear

    Scenario: Create a new shipment with already exists code
        Given a shipment sub type with the following details
            | Code         | Air                                      |
            | Name         | Testing Shipment Sub Type Daily Scenario |
            | ShipmentType | Air                                      |
        When create shipment sub type
        Then the shipment sub type should not create successfully
        And a validation error with "Code already exists" message should appear

    Scenario: Create a new shipment sub type
        Given a shipment sub type with the following details
            | Code         | Random                                   |
            | Name         | Testing Shipment Sub Type Daily Scenario |
            | ShipmentType | Air                                      |
        When create shipment sub type
        Then the shipment sub type should create successfully

    Scenario: Search for the shipment sub type
        When search for shipment sub type
        Then the shipment sub type should appear successfully

    Scenario: Open the shipment sub type
        When open shipment sub type
        Then the shipment sub type should open successfully

    Scenario: Edit the shipment sub type
        Given "Testing Edit Shipment Sub Type Daily Scenario" as shipment sub type name
        And the user inactivate the shipment sub type
        When update shipment sub type
        Then the shipment sub type should update successfully
        And the following event should appear in events tab
            | Event   |
            | Updated |