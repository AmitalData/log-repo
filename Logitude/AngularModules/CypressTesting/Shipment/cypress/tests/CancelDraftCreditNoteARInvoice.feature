Feature: Credit Note ARInvoice Operations And Actions
    This feature file will create a direct shipment and assign an Credit Note ARInvoice, cancel draft


    Scenario: Create Direct Export Air Shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | E         | A             | Shipper1 | LHR                  | MIA                |
        When create shipment
        Then the shipment should create successfully

    Scenario: Create credit note ARInvoice
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | -10       | EUR      | 4            |
        And a credit ARInvoice with the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      | VATType |
            | Customer    | EUR             | 4                   | today       | Cash         | today   | Zero  | Main Office | Zero    |
        When create invoice
        Then the invoice should create successfully

    Scenario: Cancel draft of credit note ARInvoice
        When cancel draft
        Then the invoice should cancel successfully
