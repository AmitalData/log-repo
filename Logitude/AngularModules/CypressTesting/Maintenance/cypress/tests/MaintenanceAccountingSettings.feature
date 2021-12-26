@devrelease
Feature: Accounting Settings Search and Edit from Maintenance
    The user searches for the Accounting Settings, and edits it from the Maintenance Module.
    #Background: the user logged in and open "Accounting Settings" in maintenance menu
    Scenario: Edit accounting receivable in accounting settings
        Given the user logged in and navigate to "Accounting Settings" in maintenance menu
        And the user update Receivables Accounting Settings as following
            | VoidARInvoice | Not Allowed |
            | voidARPayment | Not Allowed |
        When the user save the changes
        Then the new settings should saved successfully

    Scenario: Edit account payables in accounting settings
        Given the user navigates to "Accounting Settings" in maintenance menu
        And the user update Payables Accounting Settings as following
            | VoidAPInvoice | Not Allowed |
            | voidAPPayment | Not Allowed |
        When the user save the changes
        Then the new settings should saved successfully

    Scenario: Edit others in accounting settings
        Given the user navigates to "Accounting Settings" in maintenance menu
        When the user updates the others with "888888888888888888888" as VATNumber
        Then a validation message with "VAT Number Field must be less than 20" error should appear

