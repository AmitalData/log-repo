@release @all @dev
Feature: Multi Vat Type Create, Search and Edit from Maintenance
    The user creates a multi vat type, searches for and edits it from the Maintenance Module.

    Scenario: Create new multi vat type
        Given the user logged in and open "VatTypes" in maintenance menu
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
        When search multi vat type
        Then the multi vat type should appear successfully

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

    Scenario: Save and close the multi vat type
        When save and close multi vat type
        Then the multi vat type should close successfully