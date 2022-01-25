@devrelease
Feature: Automations Create, and Edit from Automations
    The user creates Automations on update and on create,and edit it from the Maintenance Module.

    Scenario: Create new automation OnUpdate with set field value result
        Given the user logged in
        And the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area     | Entity   | Type    | Field                   | Operator   |
            | and Area | Shipment | boolean | Is Operationally Closed | IsnotEmpty |
        And the use adds Set Fields Value result
            | Field         | Customs Clearance Date |
            | Fieldtype     | date                   |
            | Operator      | Set Constant Value     |
            | Operatorvalue | Date                   |
            | Value         | 23/01/2022             |
        When create automation
        Then the new automation should create successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the direct should create successfully

    Scenario: Update shipment general tab
        Given the user fill "100" as ValueOfGoods and "MTA" as a MoveType
        When update shipment
        Then the direct should update successfully
        And the Automation should executed successfully

    Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: Create new automation OnUpdate with FU Creation result
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Master Automation  |
            | Description | Master Description |
        And the user add the following condition
            | Area     | Entity   | Type | Field                     | Operator | OperatorValue | ConditionValue |
            | and Area | Shipment | date | Actual Final Arrival Date | equals   | todayPlus     | 5              |
            | and Area | Master   | date | Main Carriage ATD         | equals   | todayPlus     | 5              |
        And the user adds FU Creation result
            | Type         | Immediately       |
            | FollowUpType | Invoice Prepared  |
            | Owner        | Update By         |
            | Date         | Main Carriage ETD |
            | Notes        | BDD Notes         |
        When create automation
        Then the new automation should create successfully

            Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: Create new automation OnUpdate with set field value result
        Given the user navigates to automations menu
        And the user select "Masters" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Master Automation  |
            | Description | Master Description |
        And the user add the following condition
            | Area     | Entity | Type | Field             | Operator | OperatorValue | ConditionValue |
            | and Area | Master | date | Main Carriage ATD | equals   | todayPlus     | 5              |
        And the use adds Set Fields Value result
            | Field         | Booking Conf #           |
            | Fieldtype     | number                 |
            | Operator      | Set Value From [Field] |
            | Operatorvalue | no                     |
            | Value         | Master Shipment No.        |
        When create automation
        Then the new automation should create successfully

            Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Masters" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: Create new automation onCreate with Email result
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "onCreate" with automation detailes as following
            | Name        | Master Automation  |
            | Description | Master Description |
        And the user add the following condition
           | Area     | Entity   | Type | Field                     | Operator | OperatorValue | ConditionValue |
            | and Area | Master   | date | Main Carriage ATD         | equals   | todayPlus     | 5              |
       
        And the use adds Email result
            | Type             | Immediately          |
            | Document         | Shipping Declaration |
            | Template         |                      |
            | DocumentTemplate |                      |
            | Recipients       | Salesman             |
        When create automation
        Then the new automation should create successfully

    Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "onCreate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully