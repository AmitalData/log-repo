@release @all @smoke
Feature: Cancel Quote
    The user creates a quote, cancels quote and assert quote appears in cancelled quotes queries

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

    Scenario: Cancel quote
        Given the user open the quote
        When "Cancel Quote" action with "Quote Cancelled" note
        Then following event should appear in events tab
            | Event        | Notes           |
            | Cancel Quote | Quote Cancelled |
        And the quote should appear in Cancelled quotes list