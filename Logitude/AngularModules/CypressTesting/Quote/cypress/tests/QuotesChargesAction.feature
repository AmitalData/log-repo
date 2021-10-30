@release @all @stable @smoke @smoke3
Feature: Create Quote, Delete all charges and add charge
    The user creates a quote, delete all charges and add charge

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

    Scenario: Delete all charges
        Given the user open the quote
        And the user delete all charges
        When update quote
        Then the quote should update successfully

    Scenario: Add charge with fixed sale currency mode
        Given change sales currency to "NIS"
        And the user add charge with "AFT" as charge type
        When update quote
        Then the quote should update successfully
        And sale currency should have "NIS" value

    Scenario: Change currency mode to same as cost
        Given the user change currency mode to same as cost currency
        When update quote
        Then the quote should update successfully
        And sale currency should have "EUR" value
