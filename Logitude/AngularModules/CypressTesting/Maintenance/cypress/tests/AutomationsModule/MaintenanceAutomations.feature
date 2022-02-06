@devrelease
Feature: Automations Create, and Edit from Automations
    The user creates Automations on update and on create,and edit it from the Automations Module,and test the automation from shipment side.

    Scenario: Create new automation OnUpdate with set field value result
        Given the user logged in
        And the user navigates to automations menu
        And the user select "Masters" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Master Automation  |
            | Description | Master Description |
        And the user add the following condition
            | Area | Entity | Type    | Field                   | Operator   | OperatorValue | ConditionValue |
            | And  | Master | date    | Main Carriage ATD       | equals     | todayPlus     | 5              |
            | And  | Master | boolean | Is Operationally Closed | IsnotEmpty | no            | 0              |
        And the use adds Set Fields Value result
            | Field         | Booking Conf #         |
            | Fieldtype     | number                 |
            | Operator      | Set Value From [Field] |
            | Operatorvalue | no                     |
            | Value         | Master Shipment No.    |
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
            | Area | Entity | Type | Field             | Operator | OperatorValue | ConditionValue |
            | And  | Master | date | Main Carriage ATD | equals   | todayPlus     | 5              |

        And the use adds Email result
            | Type       | Immediately          |
            | Document   | Shipping Declaration |
            | Recipients | Salesman             |
        When create automation
        Then the new automation should create successfully

    Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "onCreate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully


