@release @all @dev 
Feature: Edit, Print and Send Quotation

    The user creates a quote, prints it, edits it, and sends it to a customer.

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

    Scenario: Print quotation
        Given the user in quote quotation
        When print the quotation
        Then a new page should open successfully

    Scenario: Edit quotation
        When the user add "Fixed Price" data field to quotation introduction
        Then the quotation should update successfully
        And quote stage status should be "Draft"

    Scenario: Send quotation to customer
        When the user send quotation to the logged in user
        Then the quotation should send successfully
        And quote stage status should be "Sent"