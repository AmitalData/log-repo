@smoke @CloudSmokeTestingTag
Feature: Cheque Deposit
    The user creates new AR Payment and creates new Cheque deposit

    Scenario: Create new Cheque Deposit
        Given the user logged in and navigates to Full Accounting workspace
        And an AR Payment with the following details
            | AccountingDate | TODAY  |
            | CashBook       | NisBox |
            | BankAccount    | 4sq6acrd4ibh6d |
            | ForeignAmount  | 8      |
        When create AR Payment
 
    Scenario: Approve the Cheque Deposit
             | Partner         | HadiNewCustomer2023    |
            | RegisterDate    | TODAY                  |
            | PaymentCurrency | NIS                    |
            | PaymentMethod   | Cheque                 |
            | PaymentAmount   | 1000                   |
            | PaymentBranch   | ChequeDepositBranchBDD |
        

    Scenario: Approve the AR Payment
 
        Given a cheque with the following details
             | ChequeAmount     | 1000       |
            | ChequeValueDate  | TODAY      |
            | ChequeRef        | 1235       |
            | ChequeBank       | 6958       |
            | ChequeBankBranch | 784        |
            | ChequeAccount    | 165        |
            | ForeignAmount    | 100        |    
        When Approve the AR Payment
        Then the AR Payment should approve successfully


      
     