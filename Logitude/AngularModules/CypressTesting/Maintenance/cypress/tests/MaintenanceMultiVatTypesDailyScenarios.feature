@newdev @daily
Feature: Multi Vat Type fake Create, Search and Edit from Maintenance
    The user creates a multi vat type, searches for and edits it from the Maintenance Module.

    Scenario: Add Multi Vat Type Code with lenght more than 5
        Given the user logged in and open "VatTypes" in maintenance menu
        When add "123456" as multi vat type code
        Then a validation message with "Code Field must be less than 5" error should appear

    Scenario: Create new multi vat type
        And a multi vat type with the following details
            | IsMultiPercentage | YES                             |
            | Code              | Random                          |
            | Name              | multiVatType                    |
            | LocalName         | Testing multi vat type Scenario |
            | Description       | Description                     |
            | LocalDescription  | LocalDescription                |
        When create multi vat type
        Then the multi vat type should create successfully

    Scenario: Search for the multi vat type by code
        When search for "ZERO" multi vat type
        Then the "ZERO" multi vat type should appear successfully

    Scenario: Open the multi vat type
        When open multi vat type
        Then the multi vat type should open successfully

    Scenario: Edit the multi vat type
        Given the user fill the following multi vat type General details
            | InActive         | YES                   |
            | Description      | New Description       |
            | LocalDescription | Local New Description |
        And fill the following multi vat type Accounting details
            | AccountingReceivablesExternalID | Receivables1 |
            | AccountingPayablesExternalID    | Payables1    |
        When save multi vat type
        Then the multi vat type should update successfully
        And the following event should appear in events tab
            | Event            |
            | Vat Type Updated |

    Scenario: Save and close the multi vat type
        When save and close multi vat type
        Then the multi vat type should close successfully