@release @stable @weekly
Feature: Shipping Agent Create and Edit it in Maintenance Module
    The user creates a shipping agent and edits it from the Maintenance Module.

    Scenario: Add shipping agent city with lenght more than 25
        Given the user logged in and navigate to "Shipping Agents" in maintenance menu
        When add "012345678901234567890123456789" as city
        Then a validation message with "City Field must be less than 25" error should appear

    Scenario: Assert create shipping agent without compnay name
        Given the user fill the required fields except the company
        When create shipping agent
        Then a validation single message with "Name Field is Required" error should appear

    Scenario: Create a new shipping agent
        And a shipping agent with the following details
            | CompanyName | Testing Shipping Agent Weekly Scenario |
            | LocalName   | Testing Shipping Agent Weekly Scenario |
            | Address1    | 15 Shipping Agent Street               |
            | City        | Anchorage                              |
            | Country     | United States                          |
            | State       | Alaska                                 |
            | Zip         | 0000                                   |
            | Phone       | 0590000000                             |
            | Fax         | 0590000000                             |
        And a shipping agent contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | BusinessPhone | 0590000000  |
            | Mobile        | 0590000000  |
            | Fax           | 0590000000  |
            | Position      | Developer   |
        When create shipping agent
        Then the shipping agent should create successfully

    Scenario: Search for the shipping agent by code
        When search shipping agent
        Then the shipping agent should appear successfully

    Scenario: Open the shipping agent
        When open shipping agent
        Then the shipping agent should open successfully
        And the shipping agent address should have the following details
            | Address1 | 15 Shipping Agent Street |
            | City     | Anchorage                |
            | Country  | United States            |
            | State    | Alaska                   |
            | Zip      | 0000                     |
            | Phone    | 0590000000               |
            | Fax      | 0590000000               |
        And the shipping agent contact should have the following details
            | EnglishName   | TestContact |
            | BusinessPhone | 0590000000  |
            | Mobile        | 0590000000  |
            | Fax           | 0590000000  |
            | Position      | Developer   |

    Scenario: Edit the shipping agent
        Given "Test edit shipping agent" as shipping agent notes
        And inactivate the shipping agent
        And fill the following shipping agent Billing details
            | BankName | Shipping Agent Bank |
            | IBANNo   | zero Bank           |
        When update shipping agent
        Then the shipping agent should update successfully
        And the following event should appear in events tab
            | Event                  | Notes                      |
            | Shipping Agent Updated | Shipping Agent Inactivated |