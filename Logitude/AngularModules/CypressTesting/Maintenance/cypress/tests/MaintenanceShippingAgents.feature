@release @all @dev
Feature: Shipping Agent Create, Edit and Inactivate in Maintenance Module
    The user creates a shipping agent and edits it from the Maintenance Module.

    Scenario: Create new shipping agent
        Given the user logged in and navigate to "Shipping Agent" in maintenance menu
        And a shipping agent with the following details
            | CompanyName | Testing Shipping Agent Scenario |
            | LocalName   | Testing Shipping Agent Scenario |
            | Address1    | 15 Shipping Agent Street        |
            | City        | Anchorage                       |
            | Country     | United States                   |
            | State       | Alaska                          |
            | Zip         | 0000                            |
            | Phone       | 9999999999                      |
            | Fax         | 999999                          |
        And a shipping agent contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | Position      | Developer   |
            | BusinessPhone | 8888888888  |
            | Mobile        | 8888888888  |
            | Fax           | 888888888   |
        When create shipping agent
        Then the shipping agent should create successfully

    Scenario: Search for the shipping agent by code
        When search shipping agent
        Then the shipping agent should appear successfully

    Scenario: Open the shipping agent
        When open shipping agent
        Then the shipping agent should open successfully

    Scenario: Edit the shipping agent
        Given the user fill the following shipping agent details
            | Notes | Test edit shipping agent |
        And fill the following shipping agent Billing details
            | BankName | shipping agent Bank |
            | IBANNo   | zero Bank           |
        When update shipping agent
        Then the shipping agent should update successfully