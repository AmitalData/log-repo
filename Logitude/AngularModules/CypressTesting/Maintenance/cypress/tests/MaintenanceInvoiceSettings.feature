@release @dev @all 
Feature: Disable Then Enable Voiding Invoice from Maintenance 
    The user disables voiding invoices from Invoice Settings, creates AR invoice,
    tries to void it but gets an error, goes back to invoice settings in maintenance,
    enables voiding invoices, goes back to the same invoice and void it successfully

    Scenario: Disable void invoice settings
        Given the user logged in and navigate to "invoice settings" in maintenance menu
        Given accounting settings with the following details
            | VoidInvoice | Not Allowed |
        When update invoice settings
        Then the invoice setting should update successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        Given a direct shipment with the following details
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

    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Void ARInvoice
        When void invoice
        Then the following message "Accounting Settings doesn't allow void A/R Invoice" should appear

    Scenario: Enable void invoice settings
        Given the user navigates to "invoice settings" in maintenance menu
        Given accounting settings with the following details
            | VoidInvoice | Allowed |
        When update invoice settings
        Then the invoice setting should update successfully

    Scenario: Void ARInvoice
        Given the user goes back to ARInvoice
        When void invoice
        Then the invoice should void successfully