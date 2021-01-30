Feature: APInvoice operations and actions
    this feature file will create an APInvoice then approve invoice then cancel the approvement

    Scenario: Login And Open Shipments Workspace
        Given the user logged in
        And navigate to shipments workspace

    Scenario: Create Direct Export Air Shipment
        Given a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | E         | A             | Shipper1 | LHR                  | MIA                |
        When create shipment
        Then the shipment should create successfully

    Scenario: Create APInvoice
        Given a payable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency |
            | AFT         | GRWT | 5        | 10        | EUR      |
        And an ap invoice with the following details
            | Vendor     | InvoiceNumber | InvoiceAmount | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATType |
            | TestVendor | RandomNumber  | 50            | EUR             | 4                   | today       | Cash         | today   | Zero    |
        When receive invoice
        Then the invoice should create successfully

    Scenario: Approve APInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Cancel the APInvoice Approvement
        When cancel the invoice Approvement
        Then the invoice should cancel successfully
