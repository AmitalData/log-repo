@release  @all@tests
Feature: Create Consolidation Invoice
    After the user logging in the system and navigate to customers workspace
    will create a customer as shipper in the new shipment, after update packages and payables tabs,
    generate receivables from payables and create an ARInvoice and connect it to consolidation invoice ,
    create second shipment for the same shipper, add receivable, create another ARInvoice and coonect
    it to the same consolidation, approve it and pay it

    Scenario: Create customer
        Given the user logged in and navigates to customers workspace
        And a customer with the following details
            | CompanyName | City | Country | State |
            | TestCompany | LAS  | US      | AK    |
        When create customer
        Then the customer should create successfully

    Scenario: Update customer
        Given the user in the customer's billing tab
        When activate consolidated invoice option
        Then the customer should update successfully
        
    Scenario: Update Accounting System
        Given accounting System as "None"
        When change the accounting system
        Then the accounting system should update successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper     | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | TestCompany | LHR                  | MIA                |
        When create shipment
        Then the direct should create successfully

    Scenario: Update packages tab
        Given the user add package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 5        | 1      | 2     | 3      | 100         |
        When update shipment
        Then the direct should update successfully

    Scenario: Add Payables
        Given a payable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate | Vendor     |
            | AFT         | GRWT | 5        | 10        | EUR      | 4            | TestVendor |
        When add payables
        Then the payables should add successfully

    Scenario: Generating receivables from payables
        When generate receivables from payables
        Then the receivables should generate successfully

    Scenario: Create ARInvoice
        Given an ARInvoice with a random invoice number and the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      | VATType | IsConstituent |
            | Customer    | EUR             | 4                   | Today       | Cash         | Today   | Zero  | Main Office | Zero    | Yes           |
        When create invoice
        Then the invoice should create successfully

    Scenario: Create a new consolidation invoice
        Given a consolidation invoice with the following details
            | PartnerType | Partner     | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      |
            | Customer    | TestCompany | EUR             | 4                   | Today       | Cash         | Today   | Zero  | Main Office |
        When create consolidation invoice
        Then the consolidation invoice should create successfully

    Scenario: Create direct export air shipment
        Given the user back to Accounting workspace
        And the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper     | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | TestCompany | LHR                  | MIA                |
        When create shipment
        Then the direct should create successfully
    Scenario: Add receivable
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | 10        | EUR      | 4            |
        When add receivable
        Then the receivable should add successfully
    Scenario: Create ARInvoice
        Given an ARInvoice with a random invoice number and the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      | VATType | IsConstituent |
            | Customer    | EUR             | 4                   | Today       | Cash         | Today   | Zero  | Main Office | Zero    | Yes           |
        When create invoice
        Then the invoice should create successfully
    Scenario: Edit Consolidation Invoice
        Given the user navigates to draft consolidation invoice
        When edit the invoice
        Then the invoice should update successfully

    Scenario: Approve Consolidation Invoice
        When approve consolidation invoice
        Then the consolidation invoice should approve successfully

    Scenario: Pay consolidation invoice
        Given a payment with the following details
            | PartnerType | Partner           | BillToAddress | PaymentCurrency | RegisterDate | PaymentMethod | PaymentAmount |
            | Customer    | TestShipperExport | Main Address  | EUR             | Today        | Cash          | 50            |
        When pay the consolidation invoice
        Then the consolidation invoice should pay successfully

    Scenario: Approve payment
        When approve the payment
        Then the payment should approve successfully