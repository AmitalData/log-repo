@release @all @dev @daily 
Feature: Trucker Mock Create and Edit in Maintenance Module
    The user creates a trucker mock create and edit another one from the Maintenance Module.

    Scenario: Create a new trucker
        Given the user logged in and navigate to "Truckers" in maintenance menu
        And a trucker with the following details
            | Code        | TSCode                         |
            | CompanyName | Testing Trucker Daily Scenario |
            | LocalName   | Testing Trucker Daily Scenario |
            | Address1    | 15 Trucker Street              |
            | City        | Anchorage                      |
            | Country     | United States                  |
            | State       | Alaska                         |
            | Zip         | 0000                           |
            | Phone       | 0590000000                     |
            | Fax         | 0590000000                     |
        And a trucker contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | BusinessPhone | 0590000000  |
            | Mobile        | 0590000000  |
            | Fax           | 0590000000  |
            | Position      | Developer   |
        When create trucker
        Then the trucker should create successfully

    Scenario: Search for the trucker
        When search for "TestTLONTrucker" trucker
        Then the "TestTLONTrucker" trucker should appear successfully

    Scenario: Open the trucker
        When open trucker
        Then the trucker should open successfully

    Scenario: Edit the trucker
        Given the user fill the following trucker general details
            | Notes | Test edit trucker |
        And fill the following trucker Billing details
            | BankName | Trucker Bank |
            | IBANNo   | zero Bank    |
        When update trucker
        Then the trucker should update successfully
        And the following event should appear in events tab
            | Event           |
            | Trucker Updated |