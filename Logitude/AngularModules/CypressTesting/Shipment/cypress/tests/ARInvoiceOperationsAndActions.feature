@stable @smoke @release
Feature: ARInvoice operations and actions
  After the user logging in the system and navigate to shipments workspace
    will create a direct shipment,update routing tab,packages.
    add payables, generate receivables from payables, create and approve an ARInvoice,
    set as sent and void the invoice.

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
    Scenario: Add Charges in Receivables by generating from payables
        When generate receivables from payables
        Then the receivables should generate successfully
  Scenario: Create ARInvoice
        Given an ARInvoice with a random invoice number and the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      | VATType |
            | Customer    | EUR             | 4                   | Today       | Cash         | Today   | Zero  | Main Office | Zero    |
        When create invoice
        Then the invoice should create successfully
    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Set ARInvoice as sent
        When set invoice as sent
        Then the invoice should set as sent successfully

    Scenario: Void ARInvoice
        When void invoice
        Then the invoice should void successfully