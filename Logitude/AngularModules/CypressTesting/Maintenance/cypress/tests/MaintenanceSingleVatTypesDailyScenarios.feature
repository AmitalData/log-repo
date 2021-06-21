@newdev @daily
Feature: Single Vat Type fake Create, Search and Edit from Maintenance
    The user creates a single vat type, searches for and edits it from the Maintenance Module.

    Scenario: Add Single Vat Type Code with lenght more than 5
        Given the user logged in and open "VatTypes" in maintenance menu
        When add "123456" as single vat type code
        Then a validation message with "Code Field must be less than 5" error should appear

    Scenario: Create new single vat type
        Given a single vat type with the following details
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
        When search for "xvctb" single vat type
        Then the "xvctb" single vat type should appear successfully

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
            | Event            |
            | Vat Type Updated | 

    Scenario: Save and close the single vat type
        When save and close single vat type
        Then the single vat type should close successfully