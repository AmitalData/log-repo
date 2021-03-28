@release @dev @all
Feature: Change the currency rate value
    bla bla

    Scenario: Edit exchange rate value
        Given the user logged in and open "Currencies Rates" from tenant settings
        Given fill "EUR" today's exchange rate if it's not updated with the following details
            | ExchangeDate | Today   |
            | Rate         | 3.80000 |
        When edit currency rate
        Then the currency rate should update successfully
        And currency history will contain the following details
            | Currency     | EUR     |
            | ExchangeDate | Today   |
            | Rate         | 3.80000 |

    Scenario: Create export air quote
        Given the user navigates to quotes workspace
        And a quote with the following details
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create quote
        Then the quote should create successfully

    Scenario: Quote cost currency
        Given the user open the quote and navigate to Charges
        Given the user open Add Charges window
        When fill Cost Currency with "EUR"
        Then cost Exchange Rate should equal to "3.80000"

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct      |
            | Direction            | Export      |
            | TransportMode        | Air         |
            | Shipper              | TestCompany |
            | MainCarriageFromPort | LHR         |
            | MainCarriageToPort   | MIA         |
        When create shipment
        Then the direct should create successfully

    Scenario: Receivable cost currency
        Given the user open the direct shipment
        Given a receivable with the following details
            | ChargesType | AFT  |
            | UOM         | GRWT |
            | Quantity    | 5    |
            | UnitPrice   | 10   |
        When fill "EUR" as receivable Currency
        Then the Receivable Exchange Rate should equal to "3.80000"

    Scenario: ARInvoice cost currency
        Given the user add new invoicee
        When fill "EUR" as Invoice Currency
        Then the Invoice Exchange Rate should equal to "3.80000"

    Scenario: ARInvoice cost currency old date
        When fill "25/06/2019" as Invoice Date
        Then the Invoice Exchange Rate should equal to "4.00000"



