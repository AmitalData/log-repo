@smoke @CloudSmokeTestingTag
Feature: Journal
    The user creates new Journal, add new line and approve the Journal

    Scenario: Create new Journal
        Given the user logged in and navigates to Full Accounting workspace
        And navigate journal workspace
        And a journal line action with the following details
            | ActionName    | חובה+זכות |
            | RefDate       | 16/10/2022 |
            | DueDate       | 16/10/2022 |
            | CreditAccount | KHTest     |
            | DebitAccount  | KHTest     |
            | Amount        | 100        |
        And fill "10/08/2023" as accounting date
        When save as draft
        Then the journal should create successfully

    Scenario: Approve the Journal
        When  approve the journal
        Then the journal should approve successfully

    Scenario: Print report
        When print the report
        Then the report should print successfully