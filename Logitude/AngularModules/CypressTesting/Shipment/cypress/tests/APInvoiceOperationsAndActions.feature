@stable @smoke @release
Feature: APInvoice operations and actions
    After the user logging in the system and navigate to shipments workspace
    will create a direct shipment,update routing tab,packages.
    add payables, create and approve an APInvoice
    cancel the approvement and void the invoice.

    Scenario: Create customer
        Given the user logged in and navigates to customers workspace
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

    Scenario: Update routing tab
        Given the user in the shipment's rounting tab
        And edit main carriage leg with the follwing details
            | Airline | FlightNumber | MAWB   |
            | AA      | Random       | Random |
        When update shipment
        Then the direct should update successfully

    Scenario: Update packages tab
        Given the user add package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 5        | 1      | 2     | 3      | 100         |
        When update shipment
        Then the direct should update successfully
    Scenario: Add Payables
        Given a payable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency |
            | AFT         | GRWT | 5        | 10        | EUR      |
        When add payables
        Then the payables should add successfully

    Scenario: Create APInvoice
        Given an APInvoice with a random invoice number and the following details
            | Vendor     | InvoiceAmount | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATType | VatNo |
            | TestVendor | 50            | EUR             | 4                   | Today       | Cash         | Today   | Zero    | 5     |
        When receive invoice
        Then the invoice should create successfully

    Scenario: Approve APInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Cancel the APInvoice approvement
        When cancel the invoice approvement
        Then the invoice should cancel successfully

    Scenario: Void APInvoice
        When void invoice
        Then the invoice should void successfully
