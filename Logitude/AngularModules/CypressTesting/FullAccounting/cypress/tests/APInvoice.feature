@dev
Feature: AP Invoice
    The user creates new AP Invoice, add new line and approve the AP Invoice

    Scenario: Create new AP Invoice
        Given the user logged in and navigates to Full Accounting workspace
        And an AP Invoice with the following details
            | Vendor        | KHTest     |
            | InvoiceNumber | Random     |
            | InvoiceAmount | 100        |
            | InvoiceDate   | 01/09/2021 |
        When create AP Invoice
        Then the AP Invoice should get successfully

    Scenario: Add new Invoice Line
        Given Invoice line with the following details
            | ChargesType      | BDDChargeType  |
            | LocalDescription | LocalDirection |
            | VatType          | Zero           |
            | Amount           | 100            |
        When add Invoice Line
        Then the Invoice Line should be added successfully

    Scenario: Approve the AP Invoice
        When approve the AP Invoice
        Then the AP Invoice should approve successfully