@devsmoke
Feature: Vendor Create, Search, activate and Edit from Maintenance
    The user creates a vendor, searches for, activate it and edits it from the Maintenance Module.

    Scenario: Create new vendor
        Given the user logged in and open "Vendors" in maintenance menu
        And a vendor with the following details
            | CompanyName | Testing Vendor Scenario |
            | LocalName   | Testing Vendor Scenario |
            | Phone       | 9999999999              |
            | Address1    | 15 Vendor Street        |
            | City        | Anchorage               |
            | Country     | United States           |
        When create vendor
        Then the vendor should create successfully

    Scenario: Search for the vendor by code
        When search vendor
        Then the vendor should appear successfully

    Scenario: Open the vendor
        When open vendor
        Then the vendor should open successfully

    Scenario: Activate the vendor in accounting system
        Given navigates new account wizerd inside the customer
        And a GL Account with the following details
            | ChartOfAccounts    | Local Name Modified |
            | LocalName          | CurrentDate         |
            | EnglishName        | CurrentDate         |
            | Currency           | NIS                 |
            | ReconcileMethod    | מטבע מקומי          |
            | RevenueExpenseType | Other               |
        When create GL Account
        Then the GL Account should create successfully