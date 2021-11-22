@release

Feature: unexpected payable case
    The user creates a direct export air shipment, add unexpected payable.

Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct      |
            | Direction            | Export      |
            | TransportMode        | Air         |
            | Shipper              | TestCompany |
            | MainCarriageFromPort | LHR         |
            | MainCarriageToPort   | MIA         |
        When create shipment
        Then the direct should create successfully

    Scenario: Add AP invoice
        Given open the shipment and navigates to payable wizard
        And creates APInvoice with a random invoice number and the following details
            | Vendor              | TestVendor  |
            | InvoiceAmount       | 50          |
            | InvoiceCurrency     | EUR         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | Today       |
            | PaymentTerms        | Cash        |
            | DueDate             | Today       |
            | VATType             | Zero        |
            | VatNo               | 5           |
            | Branch              | Main Office |
        When create invoice  
        Then the invoice should create successfully

Scenario: Add invoice line 
        Given the user creates an AP invoice line
            | Charges Type   | AFT        |
            | VAT Type       | EXMPT      |
            | VAT Percentage | 0          |
            | Vendor         | TestVendor |
            | Amount         | 100        |
        When save the invoice 
        Then the invoice is saved
        And a Payable line is created in the payable wizard 