@release @all @dev @all
Feature: Quote Set as Sent to Customer, Return to Draft, Reactivate & Copy

    The user creates a quote, sets it as Sent to Customer, returns it to draft,
    cancels the quotes, reactivates it and then copies the quote.

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

    Scenario: Set as Sent to Customer
        Given the user open the quote
        When "Set As Sent" action with "Testing The set as sent to customer" note
        Then quote stage status should be "Sent"
        And following event should appear in events tab
            | Event      | Notes                               |
            | Quote Sent | Testing The set as sent to customer |

    Scenario: Return quote to draft
        When "Return To Draft" action with "Testing The return quote to draft" note
        Then quote stage status should be "Draft"
        And following event should appear in events tab
            | Event           | Notes                             |
            | Return To Draft | Testing The return quote to draft |

    Scenario: Cancel quote
        When "Cancel Quote" action with "Cancelling the quote to test the reactivate quote" note
        Then following event should appear in events tab
            | Event        | Notes                                             |
            | Cancel Quote | Cancelling the quote to test the reactivate quote |

    Scenario: Reactivate quote
        When "Reactivate Quote" action with "Reactivate the quote" note
        Then quote stage status should be "Draft"
        And following event should appear in events tab
            | Event            | Notes                |
            | Reactivate Quote | Reactivate the quote |

    Scenario: Copy quote
        When Copy the quote
        Then quote stage status should be "Created"
        And following event should appear in copied events tab
            | Event                     | Notes                                      |
            | Copied from another Quote | Copied from Quote number: "OldQuoteNumber" |
        And partners tab contains "TestShipperExport" as shipper
        And packages tab contains the following
            | Quantity | Length | Width | Height | GrossWeight |
            | 2        | 12     | 13    | 14     | 44.000      |
            | 3        | 44     | 34    | 22     | 678.000     |
