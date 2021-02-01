Feature: APInvoice operations and actions
    This feature file will create a direct shipment and assign an APInvoice, approve this invoice and cancel the approvement.

    Scenario: Login And Open Shipments Workspace
        Given the user logged in
        And navigate to shipments workspace

    Scenario: Create Direct Export Air Shipment
        Given a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | Shipper1 | LHR                  | MIA                |
        When create shipment
        Then the direct should create successfully

    Scenario: Create APInvoice
        Given a payable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency |
            | AFT         | GRWT | 5        | 10        | EUR      |
        And an APInvoice with the following details
            | Vendor     | InvoiceAmount | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATType |
            | TestVendor | 50            | EUR             | 4                   | today       | Cash         | today   | Zero    |
        When receive invoice
        Then the invoice should create successfully

    Scenario: Approve APInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Cancel the APInvoice approvement
        When cancel the invoice approvement
        Then the invoice should cancel successfully
