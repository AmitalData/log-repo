Feature: Credit Note ARInvoice Operations And Actions
    This feature file will create a direct shipment and assign an Credit Note ARInvoice, approve this invoice,set as sent and void invoice


    Scenario: Create Direct Export Air Shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | E         | A             | Shipper1 | LHR                  | MIA                |
        When create shipment
        Then the shipment should create successfully

    Scenario: Create credit note ARInvoice
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency |
            | AFT         | GRWT | 5        | -10       | EUR      |
        And a credit ARInvoice with the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATType |
            | Customer    | EUR             | 4                   | today       | Cash         | today   | Zero    |
        When create invoice
        Then the invoice should create successfully

    Scenario: Approve credit note ARInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Set credit note ARInvoice as sent
        When set invoice as sent
        Then the invoice should set as sent successfully

    Scenario: Void credit note ARInvoice
        When void invoice
        Then the invoice should void successfully

