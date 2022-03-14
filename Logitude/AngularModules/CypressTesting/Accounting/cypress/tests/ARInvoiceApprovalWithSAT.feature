@release
Feature: AR Invoice Approve With SAT
    The user creates a Direct Export Air shipment, creates receivable,
    creates AR Invoice, approve AR Invoice, Transfer AR Invoice to SAT and set at Transfered.

    Scenario: Update SAT Interface Settings
        Given the user logged in and navigate to SAT Interface settings
        Given SAT Interface Settings as"Profact 3.3"
        When change the SAT Interface Settings
        Then SAT Interface Settings should update successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct                     |
            | Direction            | Export                     |
            | TransportMode        | Air                        |
            | Shipper              | Cliente de prueba SA de CV |
            | MainCarriageFromPort | LHR                        |
            | MainCarriageToPort   | MIA                        |
        When create shipment
        Then the direct should create successfully


    Scenario: Add Receivable
        Given a Receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | 20        | MXN      | 4            |
        When add Receivable
        Then the Receivable should add successfully

    Scenario: Create ARInvoice
        Given an ARInvoice with the following details
            | PartnerType         | Customer    |
            | InvoiceCurrency     | MXN         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | 10/03/2022  |
            | PaymentTerms        | Cash        |
            | DueDate             | 10/03/2022  |
            | VATNo               | Zero        |
            | Branch              | Main Office |
            | VATType             | Zero        |
        When create invoice
        Then the invoice should create successfully
        And the status value should be Draft

    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully
        And And SAT status should be Transferred
          | Event                 | Notes |
          | Transferred to SAT    |       | 
    



