@smoke @smoke2
Feature: AR Payment With SAT
    The user creates new AR Payment and ARprove the AR Payment

    Scenario: Create new AR Payment
        Given the user logged in and navigates to Accounting workspace
        And an AR Payment with the following details
            | Partner         | Cliente de prueba SA de CV      |
            | PaymentCurrency | MXN                             |
            | PaymentMethod   | Cash                            |
            | PaymentAmount   | 100                             |
            | RegisterDate    | 10/03/2022                    |
            | MetodoPago      | Pago en parcialidades o diferido|
            |FormaPago        | Efectivo                        |
        When create AR Payment
        Then the AR Payment should get successfully

    Scenario: Connect Payment with Invoice 
       Given search for the specific invoice 
        When choose this invoice 
        Then the payment should be ready for sending to SAT

    Scenario: Approve the AR Payment
        When Approve the AR Payment
        Then the AR Payment should approve successfully
          
          
    Scenario: Send AR Payment To SAT
        When Send AR Payment to SAT 
        Then the AR Payment should Transferred successfully