Feature: APInvoice operations and actions
    This feature file will create a direct shipment and assign an APInvoice, approve this invoice and cancel the approvement and void invoice.


    Scenario: Create Direct Export Air Shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | TestShipperExport | LHR                  | MIA                |
        When create shipment
        Then the direct shipment should create successfully

    Scenario: Create APInvoice
        Given a payable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency |
            | AFT         | GRWT | 5        | 10        | EUR      |
        And an APInvoice with the following details and a random invoice number
            | Vendor     | InvoiceAmount | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATType |VatNo|
            | TestVendor | 50            | EUR             | 4                   | Today       | Cash         | Today   | Zero    |5|
        When receive invoice
        Then the invoice should create successfully

    Scenario: Approve APInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Cancel the APInvoice approvement
        When cancel the invoice approvement
        Then the invoice should cancel successfully
    Scenario: Void APInvoice
        When void invoice
        Then the invoice should void successfully
