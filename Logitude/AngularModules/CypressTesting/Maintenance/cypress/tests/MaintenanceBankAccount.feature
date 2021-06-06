@release @all @stable
Feature: Bank Account Create, Search and Edit from Maintenance
    The user creates a Bank Account, searches for and edits it from the Maintenance Module.

    Scenario: Create new bank account
        Given the user logged in and open "Bank Accounts" in maintenance menu
        And a bank account with the following details
            | AccountNumber | random        |
            | BankCode      | random        |
            | BranchNumber  | random        |
            | Currency      | USD           |
            | Name          | TestBank      |
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
        Given a "EditTestBankAccount" as bankAccountLocalName
        And  the user activate bank account
        When edit bank account
        Then the bank account should update successfully
        And following event should appear in events tab
            | Event                | Notes                   |
            | Bank Account Updated | Bank account Activated |