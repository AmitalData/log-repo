@release @all @smoke @smoke3
Feature: Quote Conversion
    The user creates a quote, convert quote transport mode and Convert FCL To LCL

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

    Scenario: convert quote transport mode
        Given the user open the quote
        And "ConvertQuoteTransportMode" action
        And fill the following details for quote conversion
            | TransportMode        | Ocean |
            | ShipmentType         | FCL   |
            | MainCarriageFromPort | LHR   |
            | MainCarriageToPort   | MIA   |
        When convert quote transport mode
        Then the quote should update successfully
        And following event should appear in events tab
            | Event                        |
            | Convert Quote Transport Mode |

    Scenario: Change quote type to LCL
        When "Convert to LCL" action with "Convert the quote type to LCL" note
        Then the quote should update successfully
        And following event should appear in events tab
            | Event                     | Notes                         |
            | Converted From FCL to LCL | Convert the quote type to LCL |