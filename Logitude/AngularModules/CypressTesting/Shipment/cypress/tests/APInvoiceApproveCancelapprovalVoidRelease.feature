@release
Feature: AP Invoice Approve, Cancel Approval and Void
    The user creates a Direct Export Air shipment, updates routings and packages, adds payable,
    creates AP Invoice, approves the AP Invoice, cancels the AP Invoice approval and voids the AP Invoice.

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

    Scenario: Create APInvoice
        Given an APInvoice with a random invoice number and the following details
            | Vendor              | TestVendor  |
            | InvoiceAmount       | 50          |
            | InvoiceCurrency     | EUR         |
            | InvoiceExchangeRate | 4           |
            | InvoiceDate         | Today       |
            | PaymentTerms        | Cash        |
            | DueDate             | Today       |
            | VATType             | Zero        |
            | VatNo               | 5           |
            | Branch              | Main Office |
        When receive invoice
        Then the invoice should create successfully
        And the status value should be "Waiting For Approval"

    Scenario: Assert all buttons in menu are dim except void button
        When press on menu button
        Then all buttons in menu should be dim except void button

    Scenario: Approve APInvoice
        When approve invoice
        Then the invoice should approve successfully
        And the status value should be "Approved"

    Scenario: Assert all buttons in menu are not dim except void button
        When press on menu button
        Then all buttons in menu should not be dim except void button

    Scenario: Assert invoice details screen fields after approving the invoice
        Then the details screen fields should be disabled

    Scenario: Assert link of the invoice exsit
        Given navigates payables tab
        Then the link of the invoice should be exsit

    Scenario: Payables edit screen should be dim
        Given navigates payables edit screen
        Then the payables fields should be disabled

    Scenario: Assert message window appear
        When press on delete all button
        Then "Can't delete all, some lines are connected to invoices" should appear as a message window

    Scenario: Cancel the APInvoice approvement
        Given navigates invoice workspace
        When cancel the invoice approvement
        Then the invoice should cancel successfully
        And the status value should be "Waiting For Approval"

    Scenario: Assert invoice details screen fields after cancelling the invoice
        Then the details screen fields should be enabled

    Scenario: Void APInvoice
        When void invoice
        Then the invoice should void successfully
        And the status value should be "Void"

    Scenario: Assert invoice details screen fields after voiding the invoice
        Then the details screen fields should be disabled

    Scenario: Payables edit screen should not be dim and delete payable button should appear
        Given navigates payables tab
        And navigates payables edit screen
        Then the payables fields should be enabled
        And delete payable button should appear

    Scenario: Assert message window appear
        When press on delete all button
        Then "Please note that deleting will erase all the lines with the amounts inserted" should appear as a message for confirmation