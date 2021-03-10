@not-stable
Feature:  Full Shipment Scenario
    this file will create a direct shipment,fill general,order,package tab
    create AP/AR invoices,Payment,and docs in, docs out, close operationally/Accountly

    Scenario: Create Direct Export Air Shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the shipment should create successfully

    Scenario: Update general tab
        Given the user fills "100" as ValueOfGoods and "MTA" as a MoveType
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Add orders
        Given the user add order package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 5        | 1      | 2     | 3      | 100         |
            | 5        | 10     | 20    | 30     | 200         |
        When save shipment
        Then the direct shipment should save successfully

    # Scenario: Add Partners
    #     Given  partners with following details
    #         | Consignee           | Agent     | CustomsAgentExport | CustomsAgentImport | Notify1   | Notify2   | ShipperNotExporter | ConsigneeNotImporter | FreightForwarder | Coloader  | CustomClearancePoint | Consolidator | ReleasingAgent |
    #         | TestConsigneeExport | TestAgent | TestCustomAgent   | TestCustomAgent   | TestAgent | TestAgent | TestShipperExport  | TestConsigneeExport  | TestAgent        | TestAgent | TestWarehouse        | TestAgent    | TestAgent      |
    #     When save shipment
    #     Then the direct shipment should save successfully

    Scenario: Add Packages
        Given  a Package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 5        | 1      | 2     | 3      | 100         |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Add Routing
        Given the user add new pickup
        Given add delivery with "IntegrationAgent" as a partner routing
        Given add pre carriage from port "LAS" to port "NYC"
        Given add on carriage from port "JFK" to port "MIA"
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Add Payable
        Given a payable with the following details
            | ChargesType  | AFT  |
            | UOM          | GRWT |
            | Quantity     | 5    |
            | UnitPrice    | 10   |
            | Currency     | EUR  |
            | ExchangeRate | 4    |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Create APInvoice
        And an APInvoice with the following details and a random invoice number
            | Vendor              | TestVendor  |
            | InvoiceAmount       | 50          |
            | InvoiceCurrency     | EUR         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | Today       |
            | PaymentTerms        | Cash        |
            | DueDate             | Today       |
            | VatNo               | 55          |
            | VATType             | Zero        |
            | Branch              | Main Office |
        When receive APInvoice
        Then the APInvoice should create successfully

    Scenario: Approve APInvoice
        When approve APInvoice
        Then the APInvoice should approve successfully

    Scenario: Pay APInvoice
        Given an APPayment with the following details
            | Vendor          | TestVendor  |
            | PaymentMethod   | Cash        |
            | PaymentAmount   | 50.00       |
            | PaymentCurrency | EUR         |
            | Rate            | 4           |
            | RegisterDate    | Today       |
            | Branch          | Main Office |
        When pay the APInvoice
        Then the APInvoice should pay successfully

    Scenario: Add Receivables
        Given a receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | GRWT | 5        | 10        | EUR      | 4            |
            | AFT         | GRWT | 5        | -10       | EUR      | 4            |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Create ARInvoice
        Given an ARInvoice with the following details
            | PartnerType         | Customer    |
            | InvoiceCurrency     | EUR         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | Today       |
            | PaymentTerms        | Cash        |
            | DueDate             | Today       |
            | VATNo               | Zero        |
            | Branch              | Main Office |
            | VATType             | Zero        |
        When create ARInvoice
        Then the ARInvoice should create successfully

    Scenario: Approve ARInvoice
        When approve ARInvoice
        Then the ARInvoice should approve successfully

    Scenario: Set ARInvoice as sent
        When set ARInvoice as sent
        Then the ARInvoice should set as sent successfully

    Scenario: Pay ARInvoice
        Given an ARPayment with the following details
            | PartnerType     | Customer     |
            | BillToAddress   | Main Address |
            | PaymentCurrency | EUR          |
            | RegisterDate    | Today        |
            | PaymentMethod   | Cash         |
            | PaymentAmount   | 50           |
        When pay the ARInvoice
        Then the ARInvoice should pay successfully

    Scenario: Create credit note ARInvoice
        Given a credit ARInvoice with the following details
            | PartnerType         | Customer    |
            | InvoiceCurrency     | EUR         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | Today       |
            | PaymentTerms        | Cash        |
            | DueDate             | Today       |
            | VATNo               | Zero        |
            | Branch              | Main Office |
            | VATType             | Zero        |
        When create credit ARInvoice
        Then the credit ARInvoice should create successfully

    Scenario: Approve credit note ARInvoice
        When approve credit ARInvoice
        Then the credit ARInvoice should approve successfully

    Scenario: Set credit note ARInvoice as sent
        When set credit ARInvoice as sent
        Then the credit ARInvoice should set successfully

    Scenario: Pay credit note ARInvoice
        Given a credit ARPayment with the following details
            | PartnerType     | Customer          |
            | Partner         | TestShipperExport |
            | BillToAddress   | Main Address      |
            | PaymentCurrency | EUR               |
            | RegisterDate    | Today             |
            | PaymentMethod   | Offsetting        |
            | PaymentAmount   | -50               |
        When pay the ARInvoice
        Then the ARInvoice should pay successfully

    Scenario: Send docs
        When send docs
        Then the docs should send successfully

    Scenario: Upload docs
        When upload docs
        Then the docs should upload successfully

    Scenario: Delete Attachment
        When delete Attachment
        Then the attachment should delete successfully

    Scenario: Close Direct Shipment operationally
        Given the user in the direct's shipment rounting tab
        And  edit Main Carriage Leg with the following details
            | Airline      | AA     |
            | FlightNumber | Random |
            | MAWB         | Random |
            | ATD          | Today  |
        When close shipment operationally
        Then the shipment should close successfully

    Scenario: Close Direct Shipment Accountly
        When close shipment Accountly
        Then the shipment should close successfully

    Scenario: Reopen Direct Shipment Accountly
        When reopen shipment Accountly
        Then the shipment should Reopen successfully

    Scenario: Reopen Direct Shipment operationally
        When reopen shipment operationally
        Then the shipment should Reopen successfully