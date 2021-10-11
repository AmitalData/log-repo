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

    Scenario: Accept quote
        Given the user open the quote
        When "Accept" action with "Accept the quote" note
        Then quote stage status should be "Accepted"
        And following event should appear in events tab
            | Event          | Notes            |
            | Quote Accepted | Accept the quote |
        And the quote should appear in Accept quotes list