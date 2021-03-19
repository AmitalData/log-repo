@release @c
Feature: Advanced Generic Interface AR Invoice Transfer
    The user disables the Accounting Transfer in settings, creates AR invoice,
    changes the settings for Accounting Transfer to Advanced Generic Interface,
    checks the created invoice in the Not Ready Entities in Accounting Interfaces,
    fixes the accounting external IDs validations preventing the invoice from being transferred and exports/transfers the invoice.

    Scenario: Disable accounting system
        Given the user logged in and navigate to accounting settings
        Given accounting System as "None"
        When change the accounting system
        Then the accounting system should update successfully

    Scenario: Create customer
        Given the user navigates to customers workspace
        And a customer with the following details
            | CompanyName | TestCompany |
            | City        | Anchorage   |
            | Country     | US          |
            | State       | AK          |
            | PhoneNumber | 98765443    |
            | FaxNumber   | 98765443    |
        When create customer
        Then the customer should create successfully

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

    Scenario:  Add receivable and clear external IDs (ChargesType,Currency)
        Given a receivable with the following details including clearing external IDs for chargesType and currency
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | 20        | EUR      | 4            |
        When add receivable
        Then the receivable should add successfully

    Scenario: Create ARInvoice,clear customer External Id and assert transfer status
        Given an ARInvoice with the following details
            | PartnerType         | Customer    |
            | InvoiceCurrency     | EUR         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | Today       |
            | PaymentTerms        | Cash        |
            | DueDate             | Today       |
            | VATNo               | Zero        |
            | Branch              | Main Office |
            | VATType             | Zero        |
        And clear external ID for partner
        When create invoice
        Then the invoice should create successfully
        And the transfer status should be not ready

    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Update Accounting System
        Given accounting System as "Logitude Advanced Generic Interface" and external transmission as "None"
        When update the accounting system
        Then the accounting system should update successfully
        And the ARInvoice should appear in AR Not Ready Invoices

    Scenario: Add missing external IDs
        Given an external IDs with the following details
            | ChargesType | AFT         |
            | Currency    | EUR         |
            | BillTo      | TestCompany |
        When add the external IDs
        Then the external IDs should add successfully
        And the transfer status should be ready

    Scenario: Transfer ARInvoice from transfer screen
        Given the user in transfer screen
        When export the ARInvoice
        Then the ARInvoice should export successfully
        And the transfer status should be transferred
