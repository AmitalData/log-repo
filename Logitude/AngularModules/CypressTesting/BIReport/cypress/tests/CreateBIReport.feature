Feature: BI Report
    The user creates new Bi Folder then add new BI Report and add remove columns and filters.

    Scenario: Create New BI folder
        Given the user logged in and navigates to BI Report workspace
        And  a BI folder with the following details
            | Name                  | Description                |
            | Testing Cypress Today | This folder is for testing |

        When create BI folder
        Then the folder will created successfully

    Scenario: Add Shipment BI Report to the folder
        Given User open the folder to add BI Report
        And  a Shipment BI Report with the following details
            | Name      | Testing Cypress BI Report Today |
            | FactTable | Shipments                       |
        When Create BI Report
        Then the Report will created successfully

    Scenario: Add Columns and Filters to the Report
        Given User open QueryBuilder and add some column and filter
        When add filter and cloumn user save changes
        Then the Report will create successfully with all details

    Scenario: Edit The Report
        Given User click on Edit Query to edit and add some column and filter
        When add filter and cloumn user save changes
        Then the Report will edit successfully with all details

    Scenario: Download the Report to Excel file
        When User Click On Download To Excel file Button
        Then  The report will download successfully

    Scenario: Add Shipment Charges BI Report to the folder
        Given User open the folder to add BI Report
        And  a Shipment Charges BI Report with the following details
            | Name      | Testing Cypress BI Report Today |
            | FactTable | Shipment Charges                |
        When Create BI Report
        Then the Report will created successfully

