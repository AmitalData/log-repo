@devrelease
Feature: Accounting Settings Search and Edit from Maintenance
    The user searches for the Accounting Settings, and edits it from the Maintenance Module.

    Scenario: Edit receivable and payables in accounting settings
        Given the user logged in and navigate to "Accounting Settings" in maintenance menu
        And the user update accounting settings as following
            | VoidARInvoice | yes |
            | voidARPayment | yes |
            | VoidAPInvoice | yes |
            | voidAPPayment | yes |
        When the user save the changes
        Then the new settings should saved successfully

    Scenario: Edit invoices and payment settings in accounting settings
        Given the user navigates to "Accounting Settings" in maintenance menu
        And the user update accounting settings as following
            | VoidARInvoice | No |
            | voidARPayment | No |
            | VoidAPInvoice | No |
            | voidAPPayment | No |
        When the user save the changes
        Then the new settings should saved successfully

    Scenario: Edit others in accounting settings
        Given the user navigates to "Accounting Settings" in maintenance menu
        When the user updates the others with "888888888888888888888" as VAT number
        Then a validation message with "VAT Number Field must be less than 20" error should appear

