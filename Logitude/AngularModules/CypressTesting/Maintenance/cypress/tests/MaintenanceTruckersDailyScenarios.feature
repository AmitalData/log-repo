@release @all @stable @daily
Feature: Trucker fake Create and Edit in Maintenance Module
    The user creates a trucker fake create and edit another one from the Maintenance Module.

    Scenario: Add Trucker Code with lenght more than 7
        Given the user logged in and navigate to "Truckers" in maintenance menu
        When navigate trucker wizard and add "12345678" as trucker code
        Then a validation message with "Code field must be less than 7 and more than 0" error should appear

    Scenario: Add Trucker Code already exists
        When add "TLON" as trucker code
        Then a validation code message with "This trucker already exists" should appear

    Scenario: Assert create Trucker without Code
        Given the user fill the required fields except the code
        When create trucker
        Then a validation single message with "Code Field is Required" error should appear

    Scenario: Create a new trucker
        Given a trucker with the following details
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
        Given "Test edit trucker" as trucker notes
        And inactivate the trucker
        And fill the following trucker Billing details
            | BankName | Trucker Bank |
            | IBANNo   | zero Bank    |
        When update trucker
        Then the trucker should update successfully
        And the following event should appear in events tab
            | Event           |
            | Trucker Updated |