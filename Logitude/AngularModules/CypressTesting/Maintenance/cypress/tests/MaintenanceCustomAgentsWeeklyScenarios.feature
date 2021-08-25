@release @all @stable @weekly
Feature: Custom Agent Create and Edit in Maintenance Module
    The user creates a custom agent and edit it from the Maintenance Module.

    Scenario: Add custom agent city with lenght more than 25
        Given the user logged in and navigate to "Custom Agents" in maintenance menu
        When add "012345678901234567890123456789" as city
        Then a validation message with "City Field must be less than 25" error should appear

    Scenario: Assert create custom agent without compnay name
        Given the user fill the required fields except the company
        When create custom agent
        Then a validation single message with "Name Field is Required" error should appear

    Scenario: Create a new custom agent
        Given a custom agent with the following details
            | CompanyName | Testing Custom Agent Weekly Scenario |
            | LocalName   | Testing Custom Agent Weekly Scenario |
            | Address1    | 15 Custom Agent Street               |
            | City        | Anchorage                            |
            | Country     | United States                        |
            | State       | Alaska                               |
            | Zip         | 0000                                 |
            | Phone       | 0590000000                           |
            | Fax         | 0590000000                           |
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
        When search custom agent
        Then the custom agent should appear successfully

    Scenario: Open the custom agent
        When open custom agent
        Then the custom agent should open successfully
        And the custom agent address should have the following details
            | CompanyName | Testing Custom Agent Weekly Scenario |
            | LocalName   | Testing Custom Agent Weekly Scenario |
            | Address1    | 15 Custom Agent Street               |
            | City        | Anchorage                            |
            | Country     | United States                        |
            | State       | Alaska                               |
            | Zip         | 0000                                 |
            | Phone       | 0590000000                           |
            | Fax         | 0590000000                           |
        And the custom agent contact should have the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | BusinessPhone | 0590000000  |
            | Mobile        | 0590000000  |
            | Fax           | 0590000000  |
            | Position      | Developer   |

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