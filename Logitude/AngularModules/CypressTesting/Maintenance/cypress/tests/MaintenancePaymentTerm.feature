@release @all @dev
Feature: Payment Terms Create, Search and Edit from Maintenance
    The user creates a payment term, searches for and edits it from the Maintenance Module.

    Scenario: Create new payment term
        Given the user logged in and open "Payment Terms" in maintenance menu
        And a payment term with the following details
            | Name             | TestingpaymenttermScenario1    |
            | LocalName        | Testing payment term Scenario  |
            | Code             | Random                         |
            | CurrentMonth     | Yes                            |
            | FromDate         | Invoice Date                   |
            | Days             | 30                             |
            | Description      | Payment Term Description       |
            | LocalDescription | Payment Term Local Description |
        When create payment term
        Then the payment term should create successfully

    Scenario: Search for the payment term by code
        When search payment term
        Then the payment term should appear successfully

    Scenario: Open the payment term
        When open payment term
        Then the payment term should open successfully

    Scenario: Edit the payment term
        Given the user edit the following payment term details
            | Description      | New Description       |
            | LocalDescription | New Local Description |
        And fill the following payment term Accounting External ID
            | AccountingExternalID | External1 |
        When save payment term
        Then the payment term should update successfully

    Scenario: Save and close the payment term
        When save and close payment term
        Then the payment term should close successfully