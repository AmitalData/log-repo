@smoke
Feature: Cheque Deposit
    The user creates new AR Payment and creates new Cheque deposit

    Scenario: Create new AR Payment
        Given the user logged in and navigates to Full Accounting workspace
        And an AR Payment with the following details
            | Partner         | BDDCustomer            |
            | RegisterDate    | 13/09/2021             |
            | PaymentCurrency | NIS                    |
            | PaymentMethod   | Cheque                 |
            | PaymentAmount   | 100                    |
            | PaymentBranch   | ChequeDepositBranchBDD |
        When create AR Payment
        Then the AR Payment should get successfully

    Scenario: Approve the AR Payment
        Given a cheque with the following details
            | ChequeValueDate  | 13/09/2021 |
            | ChequeRef        | 1235       |
            | ChequeBank       | 6958       |
            | ChequeBankBranch | 784        |
            | ChequeAccount    | 165        |
        When Approve the AR Payment
        Then the AR Payment should approve successfully

    Scenario: Create new Cheque Deposit
        Given the user navigates to cheque deposit wizerd
        And a cheque deposit with the following details
            | AccountingDate | 13/09/2021           |
            | CashBook       | ChequeDepositBookBDD |
            | BankAccount    | ChequeDepositBankBDD |
        When create cheque deposit
        Then the cheque deposit should get successfully

    Scenario: Approve the Cheque Deposit
        Given select the all cheques in the cheque deposit
        When Approve the cheque deposit
        Then the cheque deposit should approve successfully