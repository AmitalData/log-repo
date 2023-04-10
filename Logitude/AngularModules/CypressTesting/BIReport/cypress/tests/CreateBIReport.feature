@devrelease
Feature: BI Report
    The user creates new Bi Folder then add new BI Report and add remove columns and filters.

    Scenario: Create New BI folder
        Given the user logged in and navigates to BI Report workspace
        And  a BI folder with the following details
            | Name           | Description                |
            | Cypress Folder | This folder is for testing |

        When create BI folder
        Then the folder will created successfully

 #ShipmentsFact
    Scenario: Add Shipment BI Report to the folder
        Given User open the folder to add Shipment BI Report
        And  a Shipment BI Report with the following details
            | Name                            | FactTable |
            | Shipment Fact Cypress BI Report | Shipments |
        When Create BI Report
        Then the Shipment Report will created successfully

    Scenario: Add Columns and Filters to the Report
        Given User open QueryBuilder and add "Shipment Number" column and filter to Shipment Report
        When add filter and cloumn user save changes
        Then the Shipment Report will create successfully with all details

    Scenario: Edit The Report
        Given User click on Edit Query to edit and add "Customer" column and filter to Shipment Report
        When add new filter and cloumn user save changes
        Then the Shipment Report will edit successfully with all details

    Scenario: Download the Report to Excel file
        When User Click On Download To Excel file Button
        Then  The Shipment report will download successfully

 #ShipmentChargesFact
    Scenario: Add Shipment Charges BI Report to the folder
        Given User open the folder to add Shipment Charges BI Report
        And  a Shipment Charges BI Report with the following details
            | Name                                    | FactTable        |
            | Shipment Charges Fact Cypress BI Report | Shipment Charges |
        When Create BI Report
        Then the Shipment Charges Report will created successfully

    Scenario: Add Columns and Filters to the ShipmentCharges Report
        Given User open QueryBuilder and add "branch" column and filter to Shipment Charges
        When add filter and cloumn to Shipment Charges user save changes
        Then the Shipment Charges Report will create successfully with all details

    Scenario: Edit Shipment Charges Report
        Given User click on Edit Query to edit and add "Shipper" column and filter to Shipment Charges
        When add new filter and cloumn user save changes
        Then the Shipment Charges Report will edit successfully with all details

 #MasterFact
    Scenario: Add Master BI Report to the folder
        Given User open the folder to add Master BI Report
        And  a Master BI Report with the following details
            | Name                          | FactTable |
            | Master Fact Cypress BI Report | Masters   |
        When Create Master BI Report
        Then the Master Report will created successfully

    Scenario: Add Columns and Filters to the Master Report
        Given User open QueryBuilder and add "Master" column and filter to Master
        When add filter and cloumn to Master user save changes
        Then the Master Report will create successfully with all details

    Scenario: Edit Master Report
        Given User click on Edit Query to edit and add "Master Shipment Number" column and filter to Master
        When add new filter and cloumn user save changes
        Then the Master Report will edit successfully with all details
      
 #ARInvoicesFact
    Scenario: Add ARInvoices BI Report to the folder
        Given User open the folder to add ARInvoices BI Report
        And  a ARInvoices BI Report with the following details
            | Name                              | FactTable |
            | ARInvoices Fact Cypress BI Report |ARInvoices  |
        When Create ARInvoices BI Report
        Then the ARInvoices Report will created successfully

    Scenario: Add Columns and Filters to the ARInvoices Report
        Given User open QueryBuilder and add "AR Invoice Type" column and filter to ARInvoices
        When add filter and cloumn to ARInvoices user save changes
        Then the ARInvoices Report will create successfully with all details

    Scenario: Edit ARInvoices Report
        Given User click on Edit Query to edit and add "Invoice Branch" column and filter to ARInvoices
        When add new filter and cloumn user save changes
        Then the ARInvoices Report will edit successfully with all details

 #QuoteFact
     Scenario: Add Quote BI Report to the folder
        Given User open the folder to add Quote BI Report
        And  a Quote BI Report with the following details
            | Name                         | FactTable |
            | Quote Fact Cypress BI Report |Quotes     |
        When Create Quote BI Report
        Then the Quote Report will created successfully

    Scenario: Add Columns and Filters to the Quote Report
        Given User open QueryBuilder and add "Quote Number" column and filter to Quote
        When add filter and cloumn to Quote user save changes
        Then the Quote Report will create successfully with all details

    Scenario: Edit Quote Report
        Given User click on Edit Query to edit and add "Is Quote Data External" column and filter to Quote
        When add new filter and cloumn user save changes
        Then the Quote Report will edit successfully with all details