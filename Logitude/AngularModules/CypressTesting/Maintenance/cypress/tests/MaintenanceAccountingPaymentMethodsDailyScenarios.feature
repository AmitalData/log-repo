@stable @daily @all
Feature: Accounting Payment Method fake Create, Search and Edit from Maintenance
    The user creates a accounting payment, searches for and edits it from the Maintenance Module.

    Scenario: Add Accounting Payment Method Code with lenght more than 2
        Given the user logged in and open "Accounting Payment Methods" in maintenance menu
        When add "123" as accounting payment code
        Then a validation message with "Code Field must be less than 2" error should appear

    Scenario: Create new accounting payment method
        Given an accounting payment with the following details
            | Name       | new payment method |
            | Code       | Random             |
            | ARCheckBox | Yes                |
            | APCheckBox | Yes                |
        When create accounting payment
        Then the accounting payment should create successfully

    Scenario: Search for the accounting payment method by code
        When search for "CH" accounting payment
        Then the "Ch" accounting payment should appear successfully

    Scenario: Open the accounting payment method
        When open accounting payment
        Then the accounting payment should open successfully

    Scenario: Edit the accounting payment method
        Given the user edit the following accounting payment details
            | InactiveCheckBox | Yes |
            | APCheckBox       | No  |
        And fill the following Accounting tab details
            | ARExternalID | External1 |
            | APExternalID | External1 |
        When save accounting payment
        Then the accounting payment should update successfully

    Scenario: Save and close the accounting payment method
        When save and close accounting payment
        Then the accounting payment should close successfully