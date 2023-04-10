@devTest
Feature: Workflow add decision element
    The user open workflow list, create workflow and add decision element .

    Scenario: create workflow
        Given the user logged in and navigates to automation workspace
        And open workflows list
        And a flow with following details
            | Name        | random        |
            | Description | test workflow |
        When create flow
        Then the flow should create successfully

    Scenario: add decision element
        Given edit start configration with following details
            | Object           | Shipment            |
            | ConfigureTrigger | A record is updated |
        And add decision element with following details
            | Title          | Ocean or Other transport mode Shipment |
            | MetLabel       | Ocean Shipment                         |
            | OtherwiseLabel | Other transport mode Shipment          |
        And add condition group met with 'And' with the following details
            | Field                        | Operation | Value               |
            | Main Carriage Transport Mode | Equals    | Ocean               |
            | Profit Differences           | Not Equal | Profit Differences1 |
            | Containers Numbers           | Equal     | 5                   |
            | Description of Goods         | Ends With | xyz                 |
        When save flow
        Then the flow should save successfully

    Scenario: add secound level group in decision element
        Given add second level condition group met with 'Or' with the following details
            | Field             | Operation    | Value |
            | Create Date       | Greater Than | TODAY |
            | Chargeable Weight | Less Than    | 3.5   |
        And add third level condition group met with 'Or' with the following details
            | Field              | Operation  | Value      |
            | Department         | Not Equals | Accounting |
            | Order Gross Weight | Less Than  | 10000      |
        And add another second level condition group met with 'And' with the following details
            | Field             | Operation   | Value |
            | Accounting Closed | Equal       | False |
            | Notes             | Starts With | xyz   |
        When save flow
        Then the flow should save successfully
