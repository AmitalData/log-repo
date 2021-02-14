@stable @smoke @release
Feature: Create customs ARInvoice
    After the user logging in the system and navigate to shipments workspace
    will create a customer and direct shipment, after that update routing
    and packages tabs, create customs ARInvoice and approve it.

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

    Scenario: Create customs ARInvoice
        Given a receivable with the following details
            | ChargesType       | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | Customs Commision | GRWT | 5        | 20        | EUR      | 4            |
        And an ARInvoice with the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      | VATType |
            | Customer    | EUR             | 4                   | Today       | Cash         | Today   | Zero  | Main Office | Zero    |
        When create invoice
        Then the invoice should create successfully

    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully