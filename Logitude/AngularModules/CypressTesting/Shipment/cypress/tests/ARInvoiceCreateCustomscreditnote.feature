@release @stable 
Feature: AR Invoice Create Customs Credit Note
    The user activates customs, creates a customer, creates a Direct Export Air shipment, updates routings and packages,
     adds a receivable, creates and approves customs AR Invoice, creates and approves customs credit note.

    Scenario: Update Accounting System
        Given the user logged in
        Given accounting System as "None"
        When change the accounting system
        Then the accounting system should update successfully

    Scenario: Activate Customs Management in Shipments
        When the user activate customs settings
        Then the customs settings should activate successfully

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

    Scenario: Create customs credit note ARInvoice
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | -20       | EUR      | 4            |
        And a customs credit note ARInvoice with a random invoice number and the following details
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

    Scenario: Approve customs credit note ARInvoice
        When approve invoice
        Then the invoice should approve successfully