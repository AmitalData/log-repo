@smoke1 @release
Feature: Bank Account Create, Search and Edit from Maintenance
    The user creates a Bank Account, searches for and edits it from the Maintenance Module.

    Scenario: Assert create bank account
        Given the user logged in and open "Bank Accounts" in maintenance menu
        When create bank account
        Then a validation error message with "Account Number Field is Required" should appear
        And a validation error message with "Currency Field is Required" should appear
        And a validation error message with "Bank Code Field is Required" should appear

    Scenario: Create new bank account
        And a bank account with the following details
            | AccountNumber | Test          |
            | BankCode      | random        |
            | BranchNumber  | Test          |
            | Currency      | USD           |
            | Name          | CurrentDate   |
            | LocalName     | TestBankLocal |
        When create bank account
        Then the bank account should create successfully

    Scenario: Search for the bank account by number
        When search bank account
        Then the bank account should appear successfully

    Scenario: Open the bank account
        When open bank account
        Then the bank account should open successfully

    Scenario: Edit the Bank Account
        Given the user fill "EditBankAccountLocalNameTest" as a local name value
        And  the user activate bank account
        When edit bank account
        Then the bank account should update successfully
        And following event should appear in events tab
            | Event   | Notes |
            | Updated |       |

    Scenario: Save and close the bank account
        When save and close bank account
        Then the bank account should close successfully