@release @all @smoke
Feature: Decline Quote
    The user creates a quote, Decline quote and assert quote appears in all quotes queries with Declined stage

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

    Scenario: Decline quote
        Given the user open the quote
        When "Decline" action with "Decline the quote" note
        Then quote stage status should be "Declined"
        And following event should appear in events tab
            | Event          |
            | Quote Declined |
        And the quote should appear in all quotes list