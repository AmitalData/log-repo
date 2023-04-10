@smoke @CloudSmokeTestingTag
Feature: Chart of Account Create, Search and Edit from Maintenance
    The user creates a Chart of Account, searches for and edits it from the Maintenance Module.

    Scenario: Create new Chart of Account
        Given the user logged in and open "Chart Of Accounts" in maintenance menu
        And a Chart of Account with the following details
            | Code        | Random           |
            | EnglishName | Chart of Account |
            | LocalName   | Chart of Account |
            | Type        | Customer         |

        When create Chart of Account
        Then the Chart of Account should create successfully

    Scenario: Search for the Chart of Account by name
        When search Chart of Account
        Then the Chart of Account should appear successfully

    Scenario: Open the Chart of Account
        When open Chart of Account
        Then the Chart of Account should open successfully

    Scenario: Edit the Chart of Account
        Given the user edit the Chart of Account
        When save Chart of Account
        Then the Chart of Account should update successfully