@release @stable @weekly
Feature: Payment Terms Create, Search and Edit from Maintenance
    The user creates a payment term, searches for and edits it from the Maintenance Module.

    Scenario: Add Payment Term Method Code with lenght more than 4
        Given the user logged in and open "Payment Terms" in maintenance menu
        When add "12345" as payment term code
        Then a validation message with "Code Field must be less than 4" error should appear

    Scenario: Add Payment Term Method Code already exists
        Given add another payment term code: "CH"
        Then this validation message error "Payment Term with Code CH already exists" should appear

    Scenario: Create new payment term
        Given a payment term with the following details
            | Name             | Testing payment term Scenario  |
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
            | InactiveCheckBox | Yes                   |
        And fill the following payment term Accounting External ID
            | AccountingExternalID | External1 |
        When save payment term
        Then the payment term should update successfully
        And the following event should appear in events tab
            | Event                | Notes                    |
            | Payment Term Updated | Payment Term Inactivated |

    Scenario: Save and close the payment term
        When save and close payment term
        Then the payment term should close successfully