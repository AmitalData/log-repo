@smoke
Feature: TrailBalanceReport
    The user Run TrailBalanceReport

    Scenario: Run TrailBalanceReport
        Given the user logged in and navigates to Full Accounting workspace
        And run Trail Balance Report with the following details
            | FormDate    | CurrentDay |
            | FormDate    | CurrentDay |
            
        When run Trail Balance Report
        Then the Trail Balance Report should get successfully

   