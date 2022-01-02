@devrelease
Feature: Create, Search, Open and Edit a Report from Maintenance
    The user creates, searches for, opens and edits a quote template from Maintenance Module.

    Scenario: Create new Report
        Given the user logged in and open "Reports" in maintenance menu
        And a Report with the following details
            | Name        | random         |
            | Code        | random         |
            

        When create Report
        Then the Report should create successfully

    Scenario: Search for the Report by Code
        When search for Report
        Then the Reports should appear successfully

    Scenario: Add new message template for the report
        Given The user create a new message template with the following details
          | Description | Cypress Report |
        When save Report
        Then the message template should update successfully
            | Report Updated | Customer Inactivated |
