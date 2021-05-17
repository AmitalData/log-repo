@release @all @dev @weekly @l
Feature: Trucker Create and Edit in Maintenance Module
    The user creates a trucker and edits it from the Maintenance Module.

    Scenario:Add Trucker Code with lenght more than 7
        Given the user logged in and navigate to "Truckers" in maintenance menu
        When add "12345678" as trucker code
        Then a validation message with "Code field must be less than 7 and more than 0" error should appear

    Scenario: Create a new trucker
        Given a trucker with the following details
            | Code        | Random                          |
            | CompanyName | Testing trucker weekly Scenario |
            | LocalName   | Testing trucker weekly Scenario |
            | Address1    | 15 trucker Street               |
            | City        | Anchorage                       |
            | Country     | United States                   |
            | State       | Alaska                          |
            | Zip         | 0000                            |
            | Phone       | 0590000000                      |
            | Fax         | 0590000000                      |
        And a trucker contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | BusinessPhone | 0590000000  |
            | Mobile        | 0590000000  |
            | Fax           | 0590000000  |
            | Position      | Developer   |
        When create trucker
        Then the trucker should create successfully

    Scenario: Search for the trucker by code
        When search trucker
        Then the trucker should appear successfully

    Scenario: Open the trucker
        When open trucker
        Then the trucker should open successfully
        And the trucker address should have the following details
            | Address1 | 15 trucker Street |
            | City     | Anchorage         |
            | Country  | United States     |
            | State    | Alaska            |
            | Zip      | 0000              |
            | Phone    | 0590000000        |
            | Fax      | 0590000000        |
        And the trucker contact should have the following details
            | EnglishName   | TestContact |
            | BusinessPhone | 0590000000  |
            | Mobile        | 0590000000  |
            | Fax           | 0590000000  |
            | Position      | Developer   |

    Scenario: Edit the trucker
        Given "Test edit trucker" as trucker notes
        And fill the following trucker Billing details
            | BankName | trucker Bank |
            | IBANNo   | zero Bank    |
        When update trucker
        Then the trucker should update successfully
        And the following event should appear in events tab
            | Event           |
            | Trucker Updated |