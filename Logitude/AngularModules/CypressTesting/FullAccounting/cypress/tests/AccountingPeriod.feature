@smoke @CloudSmokeTestingTag
Feature: Accounting Period
    The user navigates to accounting periods and opens a closed month

    Scenario: Open closed accounting month
        Given the user logged in and navigates to Full Accounting workspace
        When navigate to accounting periods
        Then the accounting periods screen should be displayed
        And a year with the following details
            | Year | 2025 |
        When enter year
        Then the year should be set successfully
        And a closed month with the following details
            | PeriodType | חודש חשבונאי |
        When open closed month
        Then the month should be opened successfully

