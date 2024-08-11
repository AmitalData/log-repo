@smoke @CloudSmokeTestingTag
Feature: GL Accounts
    The user creates new GL Account and edits it

    Scenario: Create new GL Account
        Given the user logged in and navigates to GL Account workspace
        And a GL Account with the following details
            | ChartOfAccountsType | Banks      |
            | ChartOfAccounts     | Banks      |
            | LocalName           | GL         |
            | EnglishName         | GL         |
            | Currency            | NIS        |
            | ReconcileMethod     | מטבע מקומי |
            | RevenueExpenseType  | Other      |
        When create GL Account
        Then the GL Account should create successfully

    Scenario: Search for the GL Account by name
        When search GL Account
        Then the GL Account should appear successfully

    Scenario: Open the GL Account
        When open GL Account
        Then the GL Account should open successfully

    Scenario: Edit the GL Account
        Given fill "new english name" as EnglishName
        When save GL Account
        Then the GL Account should update successfully