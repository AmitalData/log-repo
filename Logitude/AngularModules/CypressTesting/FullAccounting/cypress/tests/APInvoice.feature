@smoke @CloudSmokeTestingTag
Feature: AP Invoice
    The user creates new AP Invoice, add new line and approve the AP Invoice

    Scenario: Create new AP Invoice
        Given the user logged in and navigates to Full Accounting workspace
        And an AP Invoice with the following details
            | Vendor         | KHTest     |
            | InvoiceNumber  | Random     |
            | InvoiceAmount  | 100        |
            | InvoiceDate    | 16/04/2023 |
            | AccountingDate | 16/04/2023 |
            | PaymentTerm    | Cash |
            | BranchId       | Main Office         |
        

    Scenario: Add new Invoice Line
        Given Invoice line with the following details
            | ChargesType      | עמלת עמילות   |
            | LocalDescription | LocalDirection |
            | VatType          | Zero           |
            | Amount           | 100            |
        When add Invoice Line
        Then the Invoice Line should be added successfully

    Scenario: Approve the AP Invoice
        When approve the AP Invoice
        Then the AP Invoice should approve successfully