@release @stable @daily 
Feature: Custom Agent fake Create and Edit in Maintenance Module
    The user creates a custom agent fake create and edit another one from the Maintenance Module.

    Scenario: Create a new custom agent
        Given the user logged in and navigate to "Custom Agents" in maintenance menu
        And a custom agent with the following details
            | CompanyName | Testing Custom Agent Daily Scenario |
            | LocalName   | Testing Custom Agent Daily Scenario |
            | Address1    | 15 Custom Agent Street              |
            | City        | Anchorage                           |
            | Country     | United States                       |
            | State       | Alaska                              |
            | Zip         | 0000                                |
            | Phone       | 0590000000                          |
            | Fax         | 0590000000                          |
        And a custom agent contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | BusinessPhone | 0590000000  |
            | Mobile        | 0590000000  |
            | Fax           | 0590000000  |
            | Position      | Developer   |
        When create custom agent
        Then the custom agent should create successfully

    Scenario: Search for the custom agent
        When search for "TestCustomAgent" custom agent
        Then the "TestCustomAgent" custom agent should appear successfully

    Scenario: Open the custom agent
        When open custom agent
        Then the custom agent should open successfully

    Scenario: Edit the custom agent
        Given "Test edit custom agent" as custom agent notes
        And fill the following custom agent Billing details
            | BankName | Custom Agent Bank |
            | IBANNo   | zero Bank         |
        When update custom agent
        Then the custom agent should update successfully
        And the following event should appear in events tab
            | Event                |
            | Custom Agent Updated |