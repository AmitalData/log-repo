@smoke @CloudSmokeTestingTag
Feature: Cheque Deposit
    The user creates new AR Payment and creates new Cheque deposit

    Scenario: Create new AR Payment
        Given the user logged in and navigates to Full Accounting workspace
        And an AR Payment with the following details
            | Partner         | HadiNewCustomer2023    |
            | RegisterDate    | 15/11/2023             |
            | PaymentCurrency | NIS                    |
            | PaymentMethod   | Cheque                 |
            | PaymentAmount   | 1000                   |
            | PaymentBranch   | ChequeDepositBranchBDD |
        

    Scenario: Approve the AR Payment
        Given a cheque with the following details
            | ChequeAmount     | 1000       |
            | ChequeValueDate  | 15/11/2023 |
            | ChequeRef        | 1235       |
            | ChequeBank       | 6958       |
            | ChequeBankBranch | 784        |
            | ChequeAccount    | 165        |
        When Approve the AR Payment
        Then the AR Payment should approve successfully

    Scenario: Create new Cheque Deposit
        Given the user navigates to cheque deposit wizerd
        And a cheque deposit with the following details
            | AccountingDate | 15/05/2023           |
            | CashBook       | ChequeDepositBookBDD |
            | BankAccount    | ChequeDepositBankBDD |
        When create cheque deposit
        Then the cheque deposit should get successfully

    Scenario: Approve the Cheque Deposit
        Given select the all cheques in the cheque deposit
        When Approve the cheque deposit
        Then the cheque deposit should approve successfully