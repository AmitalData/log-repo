@devsmoke
Feature: AR Payment
    The user creates new AR Payment and ARprove the AR Payment

    Scenario: Create new AR Payment
        Given the user logged in and navigates to Full Accounting workspace
        And an AR Payment with the following details
            | Partner         | TestCompany |
            | PaymentCurrency | NIS         |
            | PaymentMethod   | Cash        |
            | PaymentAmount   | 100         |
            | RegisterDate    | 01/10/2021  |
        When create AR Payment
        Then the AR Payment should get successfully

    Scenario: Approve the AR Payment
        When Approve the AR Payment
        Then the AR Payment should approve successfully