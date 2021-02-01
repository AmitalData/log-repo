Feature: ARInvoice operations and actions
    This feature file will create a direct shipment and assign an ARInvoice, cancel draft

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
        And an ARInvoice with the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATType |
            | Customer    | EUR             | 4                   | today       | Cash         | today   | Zero    |
        When create invoice
        Then the invoice should create successfully

    Scenario: Cancel draft of ARInvoice
        When cancel draft 
        Then the invoice should cancel successfully