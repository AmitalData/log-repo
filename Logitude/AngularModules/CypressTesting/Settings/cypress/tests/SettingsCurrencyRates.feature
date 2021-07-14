@release @stable @all
Feature: Edit Currency Rate Value in Settings
    The user edits the currency exchange rate and validates it in quote, receivable and invoice.

    Scenario: Edit exchange rate value
        Given the user logged in and open "Currencies Rates" from tenant settings
        And today's exchange rate for "EUR" with the following details
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

    Scenario: Check quote cost currency
        Given the user open the quote and navigate to charges
        And the user open Add charges window
        When fill cost currency with "EUR"
        Then cost exchange rate should equal "3.8"

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

    Scenario: Check receivable cost currency
        Given the user open the direct shipment
        And a receivable with the following details
            | ChargesType | AFT  |
            | UOM         | GRWT |
            | Quantity    | 5    |
            | UnitPrice   | 10   |
        When fill "EUR" as receivable currency
        Then the receivable exchange Rate should equal "3.8"

    Scenario: Cehck ARInvoice cost currency
        Given the user add a new ARInvoice
        When fill "EUR" as invoice currency
        Then the invoice exchange rate should equal "3.8"

    Scenario: Check ARInvoice cost currency in old date
        When fill "25/06/2019" as ARInvoice date
        Then the invoice exchange rate should equal "4"



