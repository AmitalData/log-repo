@NewDev
Feature: Accounting Payment Create, Search and Edit from Maintenance
    The user creates a accounting payment, searches for and edits it from the Maintenance Module.

    Scenario: Create new accounting payment
        Given the user logged in and open "Accounting Payment Methods" in maintenance menu
        And an accounting payment with the following details
            | Name       | new payment method |
            | Code       | Random             |
            | ARCheckBox | Yes                |
            | APCheckBox | Yes                |
        When create accounting payment
        Then the accounting payment should create successfully

    Scenario: Search for the accounting payment by code
        When search accounting payment
        Then the accounting payment should appear successfully

    Scenario: Open the accounting payment
        When open accounting payment
        Then the accounting payment should open successfully

    Scenario: Edit the accounting payment
        Given the user edit the following accounting payment details
            | InactiveCheckBox | Yes |
            | APCheckBox       | No  |
        And fill the following Accounting tab details
            | ARExternalID | External1 |
            | APExternalID | External1 |
        When save accounting payment
        Then the accounting payment should update successfully

    Scenario: Save and close the accounting payment
        When save and close accounting payment
        Then the accounting payment should close successfully