@smoke @stable
Feature: Credit note ARInvoice operations and actions
    After the user logging in the system and Update Accounting System to be None,navigates to shipments workspace
    will create a direct shipment, after that create and approve a credit note ARInvoice
    set as sent and void the invoice.

    Scenario: Update Accounting System
        Given the user logged in
        Given accounting System as "None"
        When change the accounting system
        Then the accounting system should update successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the shipment should create successfully

    Scenario: Create credit note ARInvoice
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | -10       | EUR      | 4            |
        And a credit ARInvoice with a random invoice number and the following details
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

    Scenario: Approve credit note ARInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Set credit note ARInvoice as sent
        When set invoice as sent
        Then the invoice should set as sent successfully

    Scenario: Void credit note ARInvoice
        When void invoice
        Then the invoice should void successfully

