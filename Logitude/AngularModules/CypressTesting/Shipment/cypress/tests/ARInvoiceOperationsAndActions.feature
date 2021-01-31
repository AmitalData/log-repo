Feature: ARInvoice operations and actions
    this feature file will create an ARInvoice then approve invoice then void the invoice for normal ARInvoice and Credit Note

    Scenario: Login And Open Shipments Workspace
        Given the user logged in
        And navigate to shipments workspace

    Scenario: Create Direct Export Air Shipment
        Given a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | E         | A             | Shipper1 | LHR                  | MIA                |
        When create shipment
        Then the shipment should create successfully

    Scenario: Create ARInvoice
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency |
            | AFT         | GRWT | 5        | 20        | EUR      |
        And an ar invoice with the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATType |
            | Customer    | EUR             | 4                   | today       | Cash         | today   | Zero    |
        When create invoice
        Then the invoice should create successfully
    Scenario: Set ARInvoice as sent
        When set invoice as sent
        Then the invoice should set as sent successfully
    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully
    Scenario: Void ARInvoice
        When void invoice
        Then the invoice should void successfully
    Scenario: Create credit note ARInvoice
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency |
            | AFT         | GRWT | 5        | -10       | EUR      |
        And a credit ar invoice with the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATType |
            | Customer    | EUR             | 4                   | today       | Cash         | today   | Zero    |
        When create invoice
        Then the invoice should create successfully
    Scenario: Set credit note ARInvoice as sent
        When set invoice as sent
        Then the invoice should set successfully
    Scenario: Approve credit note ARInvoice
        When approve invoice
        Then the invoice should approve successfully
    Scenario: Void credit note ARInvoice
        When void invoice
        Then the invoice should void successfully