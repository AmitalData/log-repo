@smoke @release @all @stable
Feature: ARInvoice Approve, set as sent and void
    After the user logging in the system and Update Accounting System to be None,navigates to shipments workspace
    will create a direct shipment,update routing tab,packages.
    add payables, generate receivables from payables, create and approve an ARInvoice,
    set as sent and void the invoice.

    Scenario: Update Accounting System
        Given the user logged in
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

    Scenario: Update routing tab
        Given the user in the shipment's rounting tab
        And edit main carriage leg with the following details
            | Airline      | AA     |
            | FlightNumber | Random |
            | MAWB         | Random |
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
            | ChargesType  | AFT  |
            | UOM          | GRWT |
            | Quantity     | 5    |
            | UnitPrice    | 10   |
            | Currency     | EUR  |
            | ExchangeRate | 4    |
        When add payables
        Then the payables should add successfully

    Scenario: Add Charges in Receivables by generating from payables
        When generate receivables from payables
        Then the receivables should generate successfully

    Scenario: Create ARInvoice
        Given an ARInvoice with a random invoice number and the following details
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

    Scenario: Set ARInvoice as sent
        When set invoice as sent
        Then the invoice should set as sent successfully

    Scenario: Void ARInvoice
        When void invoice
        Then the invoice should void successfully