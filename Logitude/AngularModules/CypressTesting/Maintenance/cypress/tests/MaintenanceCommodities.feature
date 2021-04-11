@release @dev @all
Feature: Commodity Create, Edit and Inactivate in Maintenance Module
    The user creates a commodity, edits and inactivates it from the Maintenance Module.

    Scenario: Add CommodityCode with lenght less than 4
        Given the user logged in and navigate to "Commodities" in maintenance menu
        When add "123" as Commodity code
        Then a validation message with "Code Field must be less than 15 and more than 4" error should appear

    Scenario: Add Commodity
        Given a commodity with the following details
            | CommodityCode     | random |
            | CommodityName     | random |
            | InactiveCommodity | Yes    |
        When add commodity
        Then the commodity should add successfully

    Scenario: Search for the commodity by name
        When search for commodity
        Then the commodity should appear successfully

    Scenario: Open the commodity
        When open commodity
        Then the commodity should open successfully

    Scenario: Edit the commodity
        Given a "Newcommodity" as commodityName
        And  the user activate commodity
        When edit commodity
        Then the commodity should update successfully
        And following event should appear in events tab
            | Event             | Notes               |
            | Commodity Updated | Commodity Activated |
