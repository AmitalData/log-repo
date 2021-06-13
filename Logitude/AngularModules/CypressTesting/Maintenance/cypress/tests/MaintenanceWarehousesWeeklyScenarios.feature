@newdev @weekly
Feature: Warehouses Create, Search and Edit from Maintenance
    The user creates a warehouse, searches for and edits it from the Maintenance Module.

    Scenario: Add Warehouse Code with lenght more than 5
        Given the user logged in and open "Warehouses" in maintenance menu
        When add "12345" as warehouse code
        Then a validation message with "Code field must be less than 5 and more than 0" error should appear

    Scenario: Create new warehouse
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

    Scenario: Add Terminal Code with lenght more than 25
        When add "01234567890123456789012345" as warehouse terminal code
        Then a validation message with "Terminal Code Field must be less than 25" error should appear

    Scenario: Edit the warehouse
        Given the user fill the following warehouse details
            | Notes               | Test edit warehouse |
            | Type                | Bonded              |
            | TerminalCode        | 1391                |
            | InactiveCheckBox    | Yes                 |
            | MyWarehouseCheckBox | Yes                 |
        And fill the following warehouse Billing details
            | VatNumber   | Zero             |
            | BankName    | warehouse Bank   |
            | IBANNo      | warehouse IBANNo |
            | BankAddress | pal              |
            | Swift       | swift            |
        When save warehouse
        Then the warehouse should update successfully
        And the following event should appear in events tab
            | Event             | Notes                 |
            | Warehouse Updated | Warehouse Inactivated |

    Scenario: Save and close the warehouse
        When save and close warehouse
        Then the warehouse should close successfully