@release@all @open
Feature: Generic Interface Actions
    After the user logging in the system,Update Accounting System to be None and navigate to customers workspace
    will create a customer as shipper in the new shipments
    after add receivable, create ARInvoice, clear external IDS, Aprrove ARInvoice,
    Change Accounting system to be Generic Interface, check not ready ARInvoices,
    add missing external IDs and Transfer ARInvoice

    Scenario: Update Accounting System
        Given the user logged in
        Given accounting System as "None"
        When change the accounting system
        Then the accounting system should update successfully

    Scenario: Create customer
        Given the user navigates to customers workspace
        And a customer with the following details
            | CompanyName | City | Country | State |
            | TestCompany | LAS  | US      | AK    |
        When create customer
        Then the customer should create successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper     | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | TestCompany | LHR                  | MIA                |
        When create shipment
        Then the direct should create successfully

    Scenario: Add receivable and clear ChargesType Currency External IDs
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | 20        | EUR      | 4            |
        When add receivable
        Then the receivable should add successfully

    Scenario: Create ARInvoice,clear customer External Id and assert transfer status
        Given an ARInvoice with the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      | VATType |
            | Customer    | EUR             | 4                   | Today       | Cash         | Today   | Zero  | Main Office | Zero    |
        And clear external ID for partner
        When create invoice
        Then the invoice should create successfully
        And the transfer status should be not ready

    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Update Accounting System
        Given accounting System as "Logitude Generic Interface" and external transmission as "None"
        When update the accounting system
        Then the accounting system should update successfully
        And the ARInvoice should appear in AR Not Ready Invoices

    Scenario: Add missing external IDs
        Given an external IDs with the following details
            | ChargesType | Currency | BillTo      |
            | AFT         | EUR      | TestCompany |
        When add the external IDs
        Then the external IDs should add successfully
        And the transfer status should be ready

    Scenario: Transfer ARInvoice from transfer screen
        Given the user in transfer screen
        When export the ARInvoice
        Then the ARInvoice should export successfully
        And the transfer status should be transferred
