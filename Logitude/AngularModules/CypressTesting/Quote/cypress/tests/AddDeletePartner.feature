@release @all @stable @smoke @smoke3
Feature: Create Quote, add partner and delete partner
    The user creates a quote, update partner tab and delete partner

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

    Scenario: Update partners tab
        Given the user open the quote
        And the user add the consignee partner with "TestConsigneeExport" as name
        When update quote
        Then the quote should update successfully

    Scenario: Delete partners tab
        Given the user delete the consignee partner
        When update quote
        Then the quote should update successfully