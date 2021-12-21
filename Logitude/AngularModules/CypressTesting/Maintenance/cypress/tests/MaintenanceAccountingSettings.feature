@dev @release
Feature: Accounting Settings Search and Edit from Maintenance
    The user searches for the Accounting Settings, and edits it from the Maintenance Module.
    #Background: the user logged in and open "Accounting Settings" in maintenance menu
    Scenario: Edit Accounting Receivable in Accounting Settings
        Given the user logged in and navigate to "Accounting Settings" in maintenance menu
        Given the user update Receivables Accounting Settings as following
            | VoidARInvoice | Not Allowed |
        Given the user update Receivables Accounting Settings as following
            | VoidARInvoice | Allowed |
        When the user save the changes
        Then the new settings is saved

    Scenario: Edit Account Payables in Accounting Settings
        Given the user navigates to "Accounting Settings" in maintenance menu
        Given the user update Payables Accounting Settings as following
            | VoidAPInvoice | Not Allowed |
        Given the user update Payables Accounting Settings as following
            | VoidAPInvoice | Allowed |
        When the user save the changes
        Then the new settings is saved

    Scenario: Edit Others in Accounting Settings
        Given the user navigates to "Accounting Settings" in maintenance menu
        When the user updates the others as following
            | VATNumber | 888888888888888888888 |
        Then a validation message with "VAT Number Field must be less than 20" error should appear

