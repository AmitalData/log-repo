@smoke
Feature: AR Payment
    The user creates new AR Payment and ARprove the AR Payment

    Scenario: Create new AR Payment
        Given the user logged in and navigates to Full Accounting workspace
        And an AR Payment with the following details
            | Partner         | רונן ארז דולר  |
            | RegisterDate    | 13/07/2023      |
            | PaymentCurrency | USD             |
            | PaymentMethod   | Cash            |
            | PaymentAmount   | 1000            |  
            | PaymentBranch   | Main Office     |
        When create AR Payment
        Then the AR Payment should get successfully

    Scenario: Edit print notes and Approve the AR Payment
        Given add "new notes" as print notes
        When Approve the AR Payment
        Then the AR Payment should approve successfully