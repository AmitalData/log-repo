@smoke @CloudSmokeTestingTag
Feature: Cheque Deposit
    The user creates new AR Payment and creates new Cheque deposit

    Scenario: Create new Cheque Deposit
        Given the user logged in and navigates to Full Accounting workspace
        And an AR Payment with the following details
           | AccountingDate | 13/07/2023           |
            | CashBook       | Cash                |
            | BankAccount    | 884477              |
            | ForeignAmount  | 8                   |
        When create AR Payment

    Scenario: Approve the Cheque Deposit

    Scenario: Approve the Cheque Deposit

        Given a cheque with the following details
           | AccountingDate | 13/07/2023           |
            | CashBook       | Cash                |
            | BankAccount    | 884477              |
            | ForeignAmount  | 8                   |

        When Approve the AR Payment
        Then the AR Payment should approve successfully


      
     