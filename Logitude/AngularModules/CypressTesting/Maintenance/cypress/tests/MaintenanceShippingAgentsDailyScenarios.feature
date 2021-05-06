@release @all @dev @daily
Feature: Shipping Agent Create and Edit in Maintenance Module
    The user creates a shipping agent and edit another one from the Maintenance Module.

    Scenario: Create a new shipping agent
        Given the user logged in and navigate to "Shipping Agent" in maintenance menu
        And a shipping agent with the following details
            | CompanyName | Testing Shipping Agent Daily Scenario |
            | LocalName   | Testing Shipping Agent Daily Scenario |
            | Address1    | 15 Shipping Agent Street              |
            | City        | Anchorage                             |
            | Country     | United States                         |
            | State       | Alaska                                |
            | Zip         | 0000                                  |
            | Phone       | 0590000000                            |
            | Fax         | 0590000000                            |
        And a shipping agent contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | BusinessPhone | 0590000000  |
            | Mobile        | 0590000000  |
            | Fax           | 0590000000  |
            | Position      | Developer   |
        When create shipping agent
        Then the shipping agent should create successfully

    Scenario: Search for the shipping agent
        When search for "TestShippingAgent" shipping agent
        Then the "TestShippingAgent" shipping agent should appear successfully

    Scenario: Open the shipping agent
        When open shipping agent
        Then the shipping agent should open successfully

    Scenario: Edit the shipping agent
        Given the user fill the following shipping agent details
            | Notes | Test edit shipping agent |
        And fill the following shipping agent Billing details
            | BankName | Shipping Agent Bank |
            | IBANNo   | zero Bank           |
        When update shipping agent
        Then the shipping agent should update successfully
        And the following event should appear in events tab
            | Event                  |
            | Shipping Agent Updated |