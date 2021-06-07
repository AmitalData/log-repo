@release @all @dev
Feature: Warehouses Create, Search and Edit from Maintenance
    The user creates a warehouse, searches for and edits it from the Maintenance Module.

    Scenario: Create new warehouse
        Given the user logged in and open "Warehouses" in maintenance menu
        And a warehouse with the following details
            | Code        | Random                     |
            | CompanyName | Testing warehouse Scenario |
            | LocalName   | Testing warehouse Scenario |
            | Address1    | 15 warehouse Street        |
            | Zip         | 0000                       |
            | City        | Anchorage                  |
            | Country     | United States              |
            | State       | Alaska                     |
            | Phone       | 9999999999                 |
            | Fax         | 999999                     |
        And a warehouse contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | Position      | Developer   |
            | BusinessPhone | 8888888888  |
            | Mobile        | 8888888888  |
            | Fax           | 888888      |
        When create warehouse
        Then the warehouse should create successfully

    Scenario: Search for the warehouse by code
        When search warehouse
        Then the warehouse should appear successfully

    Scenario: Open the warhouse
        When open warehouse
        Then the warehouse should open successfully

    Scenario: Edit the warehouse
        Given the user fill the following warehouse details
            | Notes | Test edit warehouse |
        And fill the following warehouse Billing details
            | VatNumber | Zero             |
            | BankName  | warehouse Bank   |
            | IBANNo    | warehouse IBANNo |
        When save warehouse
        Then the warehouse should update successfully

    Scenario: Save and close the warehouse
        When save and close warehouse
        Then the warehouse should close successfully