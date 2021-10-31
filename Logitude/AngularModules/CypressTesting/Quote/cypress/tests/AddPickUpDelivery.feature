@release @all @stable @smoke @smoke3
Feature: Create Quote, Add PickUp And Add Delivery
    The user creates a quote,add pickup and add delivery

    Scenario: Create export air quote
        Given the user logged in and navigates to quotes workspace
        And a quote with the following details
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create quote
        Then the quote should create successfully

    Scenario: Add PickUp
        Given the user open the quote
        And the user add pickup
        When update quote
        Then the quote should update successfully

    Scenario: Add Delivery
        Given the user add delivery with "Ramallah" as city and "state of Palestine" as country
        When update quote
        Then the quote should update successfully