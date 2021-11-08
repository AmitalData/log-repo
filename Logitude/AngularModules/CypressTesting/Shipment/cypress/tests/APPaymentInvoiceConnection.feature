@devsmoke @release @all
Feature: AP Payment Invoice Connection
    The user creates a vendor, creates a Direct Export Air shipment, adds payable, creates AP Invoice,
    approves the AP Invoice, create APPayment, connect the APPayment with the invoice and disconnect them

    Scenario: Create new vendor
        Given the user logged in and open "Vendors" in maintenance menu
        And a vendor with the following details
            | CompanyName | CurrentDate   |
            | City        | Anchorage     |
            | Country     | United States |
            | State       | Alaska        |
        When create vendor
        Then the vendor should create successfully

    Scenario: Create direct export air shipment
        And the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct      |
            | Direction            | Export      |
            | TransportMode        | Air         |
            | Shipper              | TestCompany |
            | MainCarriageFromPort | LHR         |
            | MainCarriageToPort   | MIA         |
        When create shipment
        Then the direct should create successfully

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
            | Vendor              | CurrentDate |
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
        And the status value should be "Waiting for Approval"
       
    Scenario: Approve APInvoice
        When approve invoice
        Then the invoice should update successfully
        And the status value should be "Approved"

    Scenario: Create new AP Payment
        Given navigates to Accounting workspace
        And an AP Payment with the following details
            | Vendor          | CurrentDate |
            | PaymentMethod   | Cash        |
            | PaymentAmount   | 50          |
            | PaymentCurrency | EUR         |
            | RegisterDate    | 01/10/2021  |
        When save the AP Payment
        Then the AP Payment should save successfully

    Scenario: Connect the invoice to the AP Payment
        Given connect the invoice to the AP Payment
        When updates the AP Payment
        Then the AP Payment should update successfully
        And the status value should be "Paid"

    Scenario: Disconnect the invoice from the AP Payment
        Given disconnect the invoice from the AP Payment
        Then the invoice should update successfully
        And the status value should be "Approved"