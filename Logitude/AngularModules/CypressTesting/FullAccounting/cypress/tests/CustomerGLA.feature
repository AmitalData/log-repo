@smoke @CloudSmokeTestingTag
Feature: Customer GLA
    The user creates new Customer and activate it in accounting system

    Scenario: Create new Customer
        Given the user logged in and navigates to Customers workspace
        And a customer with the following details
            | CompanyName | CurrentDate    |
            | Address     | Address 1      |
            | City        | London         |
            | Country     | United Kingdom |
            | Phone       | 970599000111   |
        When create customer
        Then the customer should create successfully

    Scenario: Search for the Customer by name
        When search customer
        Then the customer should appear successfully

    Scenario: Open the customer
        When open the customer
        Then the customer should open successfully

    Scenario: Activate the customer in accounting system
        Given navigates new account wizerd inside the customer
        And a GL Account with the following details
            | ChartOfAccounts       | Customer   |
            | LocalName             | GL         |
            | EnglishName           | GL         |
            | MultiCurrencyCheckBox | Yes        |
            | ReconcileMethod       | מטבע מקומי|  
            | RevenueExpenseType    | Other      |
        When create GL Account
        Then the GL Account should create successfully

    Scenario: Connect Split by Currency Accounts
        Given navigate split by currency accounts
        And fill the "NIS" as currency value
        When create GL Account
        Then the GL Account should create successfully