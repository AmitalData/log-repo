@dev
Feature: Bank Accounts
    The user creates new GL Account, new Deffered GL Account, new Transfer GL Acccount and new Bank Accounts

    Scenario: Create new GL Account
        Given the user logged in and navigates to GL Account workspace
        And a GL Account with the following details
            | ChartOfAccountsType | Banks |
            | ChartOfAccounts     | Banks |
            | LocalName           | GL    |
            | EnglishName         | GL    |
            | Currency            | NIS   |
            | RevenueExpenseType  | Other |
        When create GL Account
        Then the GL Account should create successfully

    Scenario: Create new Deffered GL Account
        Given a GL Account with the following details
            | ChartOfAccountsType | Banks      |
            | ChartOfAccounts     | Banks      |
            | LocalName           | DefferedGL |
            | EnglishName         | DefferedGL |
            | Currency            | NIS        |
            | RevenueExpenseType  | Other      |
        When create GL Account
        Then the GL Account should create successfully

    Scenario: Create new Transfer GL Acccount
        Given a GL Account with the following details
            | ChartOfAccountsType | Banks      |
            | ChartOfAccounts     | Banks      |
            | LocalName           | TransferGL |
            | EnglishName         | TransferGL |
            | Currency            | NIS        |
            | RevenueExpenseType  | Other      |
        When create GL Account
        Then the GL Account should create successfully

    Scenario: Create new Bank Account
        Given the user navigates Bank Accounts wizerd
        And a Bank Account with the following details
            | BankCode           | Leumi      |
            | BranchCode         | 1234       |
            | AccountNumber      | 1234       |
            | LocalBankName      | Local Name |
            | Currency           | NIS        |
            | GLAccount          | GL         |
            | DefferedGLAccount  | DefferedGL |
            | TransferGLAcccount | TransferGL |
        When create Bank Account
        Then the Bank Account should create successfully