@smoke
Feature: Export Currency rates
    The user Check Export Currency rates, choose dates and send to the customs

    Scenario: Check Export Currency rates
        Given the user logged in and navigates to Export workspace
        And fill Export Currency rates with the following details
            | FromDate        | Today |
            | ToDate          | Today |
            | CurrencyTypeId  | USD   |
        When Check Export Currency rates
        Then the Rates should get successfully


        Scenario: Close the test

   