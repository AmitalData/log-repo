@smoke
Feature: Journal
    The user creates new Journal, add new line and approve the Journal

    Scenario: Create new Journal
        Given the user logged in and navigates to Full Accounting workspace
        And navigate journal workspace
        And a journal line action with the following details
            | ActionName    | חובה+זכות  |
            | RefDate       | 16/10/2022 |
            | DueDate       | 16/10/2022 |
            | CreditAccount | קטרינג כהן ובניו     |
            | DebitAccount  | קטרינג כהן ובניו     |
            | Amount        | 100        |
        And fill "01/09/2021" as accounting date
        When save as draft
        Then the journal should create successfully

    Scenario: Approve the Journal
        When approve the journal
        Then the journal should approve successfully

    Scenario: Print report
        When print the report
        Then the report should print successfully