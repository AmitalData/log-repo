@release @all @stable 
Feature: AR Invoice Auto Credit
    The user creates AR Invoice, Approves it, auto credits and approves it.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the shipment should create successfully

    Scenario: Add receivable
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | 20        | EUR      | 4            |
        When add receivable
        Then the receivable should add successfully

    Scenario: Create AR invoice
        Given an AR invoice with the following details
            | PartnerType         | Customer    |
            | InvoiceCurrency     | EUR         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | Today       |
            | PaymentTerms        | Cash        |
            | DueDate             | Today       |
            | VATNo               | Zero        |
            | Branch              | Main Office |
            | VATType             | Zero        |
        When create invoice
        Then the invoice should create successfully

    Scenario: Approve AR invoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Auto credit AR invoice
        When auto credit invoice
        Then new invoice with status Auto Credit should appear
        And the AR invoice number should appear next to the auto credit invoice title
        And the auto credit invoice should have negative amount of the AR invoice

    Scenario: Approve auto credit AR invoice
        When approve auto credit invoice
        Then the auto credit invoice should approve successfully

    Scenario: Ensure the AR invoice status is Auto Credited
        When back to the AR invoice
        Then the status should be Auto Credited
        And the auto credit invoice number should appear next to the AR invoice title
