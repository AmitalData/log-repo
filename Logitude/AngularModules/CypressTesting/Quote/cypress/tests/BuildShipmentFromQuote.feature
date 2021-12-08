@release @stable @smoke @smoke3
Feature: Build shipment from quote
    The user creates a quote and build shipment from quote

    Scenario: Create export air quote
        Given the user logged in and navigates to quotes workspace
        And a quote with the following details
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        And an expected order with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 2        | 12     | 13    | 14     | 44          |
            | 3        | 44     | 34    | 22     | 678         |
        When create quote
        Then the quote should create successfully

    Scenario: Accept quote
        Given the user open the quote
        When "Accept" action with "Accept the quote" note
        Then quote stage status should be "Accepted"
        And following event should appear in events tab
            | Event          | Notes            |
            | Quote Accepted | Accept the quote |

    Scenario: Build shipment from quote
        Given build shipment from quote
        When create shipment
        Then the shipment should create successfully
