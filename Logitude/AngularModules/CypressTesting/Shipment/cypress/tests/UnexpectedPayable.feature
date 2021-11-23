@release
Feature: Unexpected Payable Case
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
        Then creates APInvoice with a random invoice number and the following details
            | Vendor              | TestVendor  |
            | InvoiceAmount       | 100         |
            | InvoiceCurrency     | EUR         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | Today       |
            | PaymentTerms        | Cash        |
            | DueDate             | Today       |
            | VATType             | Zero        |
            | VatNo               | 5           |
            | Branch              | Main Office |

    Scenario: Add invoice line
        Given the user fills AP invoice line with the following details
            | ChargesType | AFT   |
            | VatType     | EXMPT |
            | Amount      | 100   |
        When create invoice line
        Then the Invoice Line should be added successfully

     Scenario: Add invoice line
         Given the user saves the invoice
         Then a payable line is created in the payable wizard