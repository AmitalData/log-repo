Feature: ARInvoice operations and actions
    After the user logging in the system and navigate to shipments workspace
    will create a direct shipment, after that create and approve an ARInvoice
    set as sent and void the invoice.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | Shipper1 | LHR                  | MIA                |
        When create shipment
        Then the shipment should create successfully

    Scenario: Create ARInvoice
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | 20        | EUR      | 4            |
        And an ARInvoice with a random invoice number and the following details
            | PartnerType | InvoiceCurrency | InvoiceExchangeRate | InvoiceDate | PaymentTerms | DueDate | VATNo | Branch      | VATType |
            | Customer    | EUR             | 4                   | Today       | Cash         | Today   | Zero  | Main Office | Zero    |
        When create invoice
        Then the invoice should create successfully

    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully

    Scenario: Set ARInvoice as sent
        When set invoice as sent
        Then the invoice should set as sent successfully

    Scenario: Void ARInvoice
        When void invoice
        Then the invoice should void successfully