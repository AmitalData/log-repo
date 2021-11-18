@release @all @devsmoke
Feature: Shipper-Consignee Create, Search and Edit from Maintenance
    The user creates a Shipper-Consignee, searches for and edits it from the Maintenance Module.

    Scenario: Assert create Shipper-Consignee
        Given the user logged in and open "Shippers and Consignees" in maintenance menu
        And the user check add contact checkbox
        When create Shipper-Consignee
        Then a validation error message with "Name Field is Required" should appear
        Then a validation error message with "City Field is Required" should appear
        Then a validation error message with "Country Field is Required" should appear
        Then a validation error message with "English Name Field is Required" should appear

    Scenario: Create new Shipper-Consignee
        And a Shipper-Consignee with the following details
            | CompanyName | Testing Shipper-Consignee Scenario |
            | LocalName   | Testing Shipper-Consignee Scenario |
            | Address1    | 15 Shipper-Consignee Street        |
            | Zip         | 0000                               |
            | City        | Anchorage                          |
            | Country     | United States                      |
            | State       | Alaska                             |
            | Phone       | 9999999999                         |
            | Fax         | 999999                             |
        And a Shipper-Consignee contact with the following details
            | AddContact    | Yes         |
            | EnglishName   | TestContact |
            | Position      | Developer   |
            | BusinessPhone | 8888888888  |
            | Mobile        | 8888888888  |
            | Fax           | 888888      |
        When create Shipper-Consignee
        Then the Shipper-Consignee should create successfully

    Scenario: Search for the Shipper-Consignee by code
        When search Shipper-Consignee
        Then the Shipper-Consignee should appear successfully

    Scenario: Open the Shipper-Consignee
        When open Shipper-Consignee
        Then the Shipper-Consignee should open successfully

    Scenario: Edit the Shipper-Consignee
        Given a "new local name" as LocalName
        When save Shipper-Consignee
        Then the Shipper-Consignee should update successfully