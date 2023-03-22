@devrelease
Feature: AR Payment With SAT
    The user creates new AR Payment and ARprove the AR Payment

    Scenario: Update SAT Interface Settings
        Given the user logged in and navigate to SAT Interface settings
        Given SAT Interface Settings as"Profact 4.0"
        When change the SAT Interface Settings
        Then SAT Interface Settings should update successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | Cliente de prueba |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the direct should create successfully

    Scenario: Add Receivable
        Given a Receivable with the following details
            | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
            | AFT         | CHWT | 5        | 20        | MXN      | 4            |
        When add Receivable
        Then the Receivable should add successfully

    Scenario: Create ARInvoice
        Given an ARInvoice with the following details
            | PartnerType         | Customer    |
            | InvoiceCurrency     | MXN         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | 10/03/2022  |
            | PaymentTerms        | Cash        |
            | DueDate             | 10/03/2022  |
            | VATNo               | Zero        |
            | Branch              | Main Office |
            | VATType             | Zero        |
          
        When create invoice
        Then the invoice should create successfully
        And the status value should be Draft

    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully
        And And SAT status should be Transferred
            | Event              | Notes |
            | Transferred to SAT |       |
    Scenario: Create new AR Payment
        Given the user Create an AR Payment with the following details
            | Partner         | Cliente de prueba                |
            | PaymentCurrency | MXN                              |
            | PaymentMethod   | Cash                             |
            | PaymentAmount   | 1000                             |
            | RegisterDate    | 02/04/2022                       |
            | MetodoPago      | Pago en parcialidades o diferido |
            | FormaPago       | Efectivo                         |
            | Branch          | Main Office                      |
        When create AR Payment
        Then the AR Payment should get successfully

    Scenario: Connect Payment with Invoice
        Given search for the specific invoice
        When choose this invoice
        Then the payment should be ready for sending to SAT

    Scenario: Approve the AR Payment
        When Approve the AR Payment
        Then the AR Payment should approve successfully


    Scenario: Send AR Payment To SAT
        When Send AR Payment to SAT
        Then the AR Payment should Transferred successfully
            | Event              | Notes |
            | Transferred to SAT |       |

