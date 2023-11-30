@devTest
Feature: Workflow node
    The user open workflow list, open a flow, edit start configration, and save.

    Scenario: create workflow
        Given the user logged in and navigates to automation workspace
        And open workflows
        And a flow with following details
            | Name        | random        |
            | Description | test workflow |
        When create workflow
        Then the flow should create successfully

    Scenario: edit start configration
        Given edit start configration with following details
            | Object           | Shipment            |
            | ConfigureTrigger | A record is updated |
        And add condition group met with 'Or' with the following details
            | Field                   | Operation | Value               |
            | Main Carriage Final ATA | Less Than | TODAY               |
            | Incoterm                | Changed   | False               |
            | Containers Numbers      | Equal     | 5                   |
            | Agent                   | Equal     | TestAgentExport     |
            | Description of Goods    | Ends With | xyz                 |
        When save flow
        Then the flow should save successfully

    Scenario: add secound level group in start configration
        Given add second level condition group met with 'And' with the following details
            | Field             | Operation    | Value |
            | Create Date       | Greater Than | TODAY |
            | Chargeable Weight | Less Than    | 3.5   |
            | Customer          | Is Empty     | True  |
        When save flow
        Then the flow should save successfully

    Scenario: add third level group in start configration
        Given add third level condition group met with 'Or' with the following details
            | Field              | Operation  | Value      |
            | Customer           | Is Empty   | True       |
            | Department         | Not Equals | Accounting |
            | Order Gross Weight | Less Than  | 10000      |
        When save flow
        Then the flow should save successfully

    Scenario: add another second level group in start configration
        Given add another second level condition group met with 'And' with the following details
            | Field             | Operation   | Value |
            | Accounting Closed | Equal       | False |
            | Notes             | Starts With | xyz   |
        When save flow
        Then the flow should save successfully