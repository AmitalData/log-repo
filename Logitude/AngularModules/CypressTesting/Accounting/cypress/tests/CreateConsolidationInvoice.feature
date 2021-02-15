 @release  @all
Feature: Create Consolidation Invoice
    After the user logging in the system and navigate to customers workspace
    will create a customer as shipper in the new shipments,
    after update packages tab, add payables, generate receivables from payables and create an ARInvoice and connect it to
    Consolidation invoice and approve
    Scenario: Create customer
        Given the user logged in and navigates to customers workspace
        And a customer with the following details
            | CompanyName   | City | Country | State |
            | TestCompany12 | LAS  | US      | AK    |
        When create customer
        Then the customer should create successfully

    Scenario: Update customer
        Given the user in the customer's billing tab
        When activate consolidated invoice option
        Then the customer should update successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper       | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | TestCompany12 | LHR                  | MIA                |
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
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | Vendor     |
            | AFT         | GRWT | 5        | 10        | EUR      | TestVendor |
        When add payables
        Then the payables should add successfully

    Scenario: Add Charges in Receivables by generating from payables
        When generate receivables from payables
        Then the receivables should generate successfully

    Scenario: Create ARInvoice
        Given an ARInvoice with a random invoice number and the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      | VATType | IsConstituent |
            | Customer    | EUR             | 4                   | Today       | Cash         | Today   | Zero  | Main Office | Zero    | Yes           |
        When create invoice
        Then the invoice should create successfully

    Scenario:Create a new Consolidation Invoice
        Given a Consolidation Invoice with the following details
            | PartnerType | Partner       | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      |
            | Customer    | TestCompany12 | EUR             | 4                   | Today       | Cash         | Today   | Zero  | Main Office |
        When create consolidation invoice
        Then the consolidation invoice should create successfully

    Scenario:Approve Consolidation Invoice
        When approve consolidation invoice
        Then the consolidation invoice should approve successfully