@newdev
Feature: Single Vat Type Create, Search and Edit from Maintenance
    The user creates a single vat type, searches for and edits it from the Maintenance Module.

    Scenario: Create new single vat type
        Given the user logged in and open "VatTypes" in maintenance menu
        And a single vat type with the following details
            | IsSinglePercentage | YES                              |
            | Code               | Random                           |
            | Name               | singleVatType                    |
            | LocalName          | Testing single vat type Scenario |
            | Percentage         | 15                               |
            | PercentageDate     | CurrentDate                      |
            | IsRegionalTax      | YES                              |
            | Description        | Description                      |
            | LocalDescription   | LocalDescription                 |
        When create single vat type
        Then the single vat type should create successfully

    Scenario: Search for the single vat type by code
        When search single vat type
        Then the single vat type should appear successfully

    Scenario: Open the single vat type
        When open single vat type
        Then the single vat type should open successfully

    Scenario: Edit the single vat type
        Given the user fill the following single vat type General details
            | InActive         | YES                   |
            | Description      | New Description       |
            | LocalDescription | Local New Description |
        And add vat type percentage
            | PercentageDate | CurrentDate |
            | Percentage     | 4           |
        And fill the following single vat type Accounting details
            | AccountingReceivablesExternalID | Receivables1 |
            | AccountingPayablesExternalID    | Payables1    |
        When save single vat type
        Then the single vat type should update successfully
        And the following event should appear in events tab
            | Event            | Notes                |
            | Vat Type Updated | Vat Type Inactivated |

    Scenario: Save and close the single vat type
        When save and close single vat type
        Then the single vat type should close successfully