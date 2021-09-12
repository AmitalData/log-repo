@dev
Feature: AR Payment
    The user creates new AR Payment and ARprove the AR Payment

    Scenario: Create new AR Payment
        Given the user logged in and navigates to Full Accounting workspace
        And an AR Payment with the following details
            | Partner         | BDDCustomer |
            | PaymentCurrency | NIS         |
            | PaymentMethod   | Cash        |
            | PaymentAmount   | 100         |
            | PaymentBranch   | BDDBranch   |
        When create AR Payment
        Then the AR Payment should get successfully

    Scenario: ARprove the AR Payment
        When ARprove the AR Payment
        Then the AR Payment should ARprove successfully