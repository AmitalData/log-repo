@release @all @stable 
Feature: Vendor Create, Search and Edit from Maintenance
    The user creates a vendor, searches for and edits it from the Maintenance Module.

    Scenario: Create new vendor
        Given the user logged in and open "Vendors" in maintenance menu
        And a vendor with the following details
            | CompanyName | Testing Vendor Scenario |
            | LocalName   | Testing Vendor Scenario |
            | Address1    | 15 Vendor Street        |
            | Zip         | 0000                    |
            | City        | Anchorage               |
            | Country     | United States           |
            | State       | Alaska                  |
            | Phone       | 9999999999              |
            | Fax         | 999999                  |
        And a vendor contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | Position      | Developer   |
            | BusinessPhone | 8888888888  |
            | Mobile        | 8888888888  |
            | Fax           | 888888      |
        When create vendor
        Then the vendor should create successfully

    Scenario: Search for the vendor by code
        When search vendor
        Then the vendor should appear successfully

    Scenario: Open the vendor
        When open vendor
        Then the vendor should open successfully

    Scenario: Edit the vendor
        Given the user fill the following vendor details
            | Website | www.Scenario.com |
            | Notes   | Test edit vendor |
        And fill the following vendor Billing details
            | VatNumber | Zero        |
            | BankName  | Vendor Bank |
        When save vendor
        Then the vendor should update successfully

    Scenario: Save and close the vendor
        When save and close vendor
        Then the vendor should close successfully

