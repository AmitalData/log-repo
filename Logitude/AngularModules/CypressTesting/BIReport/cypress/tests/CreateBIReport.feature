Feature: BI Report
    The user creates new Bi Folder then add new BI Report and add remove columns and filters
    
    Scenario: Create New BI folder
        Given the user logged in and navigates to BI Report workspace
        And  a BI folder with the following details
            | Name        | Testing Cypress Today      |
            | Description | This folder is for testing |
        When create BI folder
        Then the folder will created successfully

    Scenario: Add BI Report to the folder
        Given User open the folder to add BI Report
        And  a BI Report with the following details
            | Name      | Testing Cypress BI Report Today |
            | FactTable | Shipments                       |
        When Create BI Report
        Then the Report will created successfully


    Scenario: Add Columns and Filters to the Report
        Given User open QueryBuilder and add some column and filter
        When add filter and cloumn user save changes
        Then the Report will create successfully with all details