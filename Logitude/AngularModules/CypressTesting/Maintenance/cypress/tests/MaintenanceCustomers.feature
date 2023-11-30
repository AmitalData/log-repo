@release
Feature: Customer Create and Edit in Maintenance Module
    The user creates a new Customer and edit it from the Maintenance Module.

    Scenario: Create a new customer
        Given the user logged in and open "Customers" in maintenance menu
        And a customer with the following details
            | CompanyName | Testing Customer Scenario |
            | LocalName   | Testing Customer Scenario |
            | Address1    | 20 Customer Street        |
            | City        | Anchorage                 |
            | Country     | United States             |
            | State       | Alaska                    |
            | Zip         | 0000                      |
            | Phone       | 059055050                 |
            | Fax         | 0590000555                |
            | VATNo       | 12345                     |
        And a customer contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | BusinessPhone | 0590000000  |
            | Mobile        | 0590000000  |
            | Fax           | 0590000000  |
            | Position      | Developer   |
        When create customer
        Then the customer should create successfully

    Scenario: Search for the customer
        When search customer
        Then the customer should appear successfully

    Scenario: Open the customer
        When open the customer
        Then the customer should open successfully
        And the customer address should have the following details
            | CompanyName | Testing Customer Scenario |
            | LocalName   | Testing Customer Scenario |
            | Address1    | 20 Customer Street        |
            | City        | Anchorage                 |
            | Country     | United States             |
            | State       | Alaska                    |
            | Zip         | 0000                      |
            | Phone       | 059055050                 |
            | Fax         | 0590000555                |

    Scenario: Edit the customer
        Given "1999" as customer StorageFreeDays
        And inactivate the customer
        And fill the following customer Billing details
            | Bankaddress | customer Bank Address |
            | IBANNo      | zero Bank             |
        When update customer
        Then the customer should update successfully
        And the following event should appear in events tab
            | Event                | Notes |
            | Customer Deactivated |       |
            | Customer Updated     |       |