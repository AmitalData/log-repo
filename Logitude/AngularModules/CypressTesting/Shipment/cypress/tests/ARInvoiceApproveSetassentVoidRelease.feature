@release
Feature: AR Invoice Approve, Set as Sent and Void
    The user creates a Direct Export Air shipment, creates receivable,
    creates AR Invoice, approve AR Invoice, set AR Invoice as sent and voids the AR Invoice.

    Scenario: Update Accounting System
        Given the user logged in
        Given accounting System as "None"
        When change the accounting system
        Then the accounting system should update successfully

    Scenario: enable void invoice settings
        Given the user navigates to "invoice settings" in maintenance menu
        Given accounting settings with the following details
            | VoidInvoice | Allowed |
        When update invoice settings
        Then the invoice setting should update successfully

    Scenario: Create customer
        Given the user navigates to customers workspace
        And a customer with the following details
            | CompanyName | TestCompany |
            | City        | Anchorage   |
            | Country     | US          |
            | State       | AK          |
            | PhoneNumber | 98765443    |
            | FaxNumber   | 98765443    |
        When create customer
        Then the customer should create successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct      |
            | Direction            | Export      |
            | TransportMode        | Air         |
            | Shipper              | TestCompany |
            | MainCarriageFromPort | LHR         |
            | MainCarriageToPort   | MIA         |
        When create shipment
        Then the direct should create successfully

    Scenario: Update routing tab
        Given the user in the shipment's rounting tab
        And edit main carriage leg with the following details
            | Airline      | AA     |
            | FlightNumber | Random |
            | MAWB         | Random |
        When update shipment
        Then the direct should update successfully

    Scenario: Update packages tab
        Given the user add package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 5        | 1      | 2     | 3      | 100         |
        When update shipment
        Then the direct should update successfully

    Scenario: Add Payables
        Given a payable with the following details
            | ChargesType  | AFT  |
            | UOM          | GRWT |
            | Quantity     | 5    |
            | UnitPrice    | 10   |
            | Currency     | EUR  |
            | ExchangeRate | 4    |
        When add payables
        Then the payables should add successfully

    Scenario: Add Charges in Receivables by generating from payables
        When generate receivables from payables
        Then the receivables should generate successfully

    Scenario: Create ARInvoice
        Given an ARInvoice with a random invoice number and the following details
            | PartnerType         | Customer    |
            | InvoiceCurrency     | EUR         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | Today       |
            | PaymentTerms        | Cash        |
            | DueDate             | Today       |
            | VATNo               | Zero        |
            | Branch              | Main Office |
            | VATType             | Zero        |
        When create invoice
        Then the invoice should create successfully
        And the status value should be "Draft"

    Scenario: Assert the status of menu buttons before approving the invoice
        When press on menu button
        Then assert the status of menu buttons before approving the invoice

    Scenario: Approve ARInvoice
        When approve invoice
        Then the invoice should approve successfully
        And the status value should be "Unpaid"

    Scenario: Assert invoice details screen fields after approving the invoice
        Then the details screen fields should be disabled

    Scenario: Assert the status of menu buttons after approving the invoice
        When press on menu button
        Then assert the status of menu buttons after approving the invoice

    Scenario: Assert link of the invoice exsit and delete receivable not exsit
        Given navigates receivables tab
        Then the link of the invoice should be exsit
        And delete receivable button should not appear

    Scenario: Receivable edit screen should be dim
        Given navigates receivables edit screen
        Then the receivables fields should be disabled

    Scenario: Set ARInvoice as sent
        Given navigates invoice workspace
        When set invoice as sent with "sent invoice" as a note
        Then the invoice should set as sent successfully
        And the following event should appear in events tab
            | Event        | Notes        |
            | Invoice Sent | sent invoice |

    Scenario: Void ARInvoice
        When void invoice
        Then the invoice should void successfully
        And the status value should be "Void"

    Scenario: Assert link of the invoice not exsit and delete receivable exsit
        Given navigates receivables tab
        Then the link of the invoice should not be exsit
        And delete receivable button should appear

    Scenario: Receivable edit screen should not be dim
        Given navigates receivables edit screen
        Then the receivables fields should be enabled