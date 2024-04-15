@smoke @CloudSmokeTestingTag
Feature: AR Invoice
    The user creates new AR Invoice, add new line and ARprove the AR Invoice

    Scenario: Create new AR Invoice
        Given the user logged in and navigates to Full Accounting workspace
        And an AR Invoice with the following details
            | BillTo          | BDDCustomer |
            | InvoiceCurrency | NIS         |
            | InvoiceDate     | 03/09/2021  |
            | PaymentTerm     | Cash        |
            | DueDate         | 03/09/2021  |
            | VATNo           | Zero        |
            | Branch          | Main Office |
            | VATType         | Zero        |
        
    Scenario: Add new Invoice Line
        Given Invoice line with the following details
            | ChargesType      | BDDChargeType  |
            | LocalDescription | LocalDirection |
            | VatType          | Zero           |
            | ForiegnCurrency  | NIS            |
            | Quantity         | 100            |
            | UnitPrice        | 100            |
        When add Invoice Line
        Then the Invoice Line should be added successfully

    Scenario: ARprove the AR Invoice
        When ARprove the AR Invoice
        Then the AR Invoice should ARprove successfully