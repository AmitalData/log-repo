@dev
Feature: Journal
    The user creates new Journal, add new line and approve the Journal

    Scenario: Create new Journal
        Given the user logged in and navigates to Full Accounting workspace
        And navigate journal workspace
        And a journal line action with the following details
            | ActionName    | חובה+זכות  |
            | RefDate       | 01/09/2021 |
            | DueDate       | 01/09/2021 |
            | CreditAccount | KHTest     |
            | DebitAccount  | KHTest     |
            | Amount        | 100        |
        When save as draft
        Then the journal should create successfully

    Scenario: Approve the Journal
        When approve the journal
        Then the journal should approve successfully