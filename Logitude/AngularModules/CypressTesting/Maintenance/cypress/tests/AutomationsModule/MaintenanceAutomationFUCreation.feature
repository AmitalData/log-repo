@devrelease
Feature: Automations Creation and Updating for Follow Up Result
    The user creates Automations on update,and edit it from the Automations Module,and test the automation from shipment side.

    #follow up result
    Scenario: FU Creation when conditions of ALL area are fit
        Given the user logged in
        And the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area | Entity   | Type    | Field                   | Operator   |
            | And  | Shipment | boolean | Is Operationally Closed | IsnotEmpty |
            | And  | Shipment | boolean | Includes Customs        | IsnotEmpty |
        And the user adds follow up creation result
            | Type         | Immediately        |
            | FollowUpType | Custom Cleared     |
            | Owner        | Salesman           |
            | Date         | Main Carriage ATD  |
            | Notes        | FU Creation result |
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

    Scenario: Update shipment to verify the automation exection
        Given the user fill "100" as ValueOfGoods and "MTA" as a MoveType
        When update shipment
        Then the direct should update successfully
        And the automation with follow up creation result should executed successfully

    Scenario: Edit an automation
        Given the user logged in
        And the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: FU Creation automation when conditions of ANY area are fit
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area | Entity   | Type    | Field                   | Operator   |
            | Or   | Shipment | boolean | Is Operationally Closed | IsnotEmpty |
            | Or   | Shipment | boolean | Includes Customs        | IsnotEmpty |
        And the user adds follow up creation result
            | Type         | Immediately        |
            | FollowUpType | Custom Cleared     |
            | Owner        | Salesman           |
            | Date         | Main Carriage ATD  |
            | Notes        | FU Creation result |
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

    Scenario: Update shipment to verify the automation exection
        Given the user fill "100" as ValueOfGoods and "MTA" as a MoveType
        When update shipment
        Then the direct should update successfully
        And the automation with follow up creation result should executed successfully

    Scenario: Edit an automation
        Given the user logged in
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: FU Creation when one of the ANY conditions is not fit
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area | Entity   | Type    | Field                   | Operator   |
            | Or   | Shipment | boolean | Is Operationally Closed | IsnotEmpty |
            | Or   | Shipment | boolean | Includes Customs        | IsEmpty    |
        And the user adds follow up creation result
            | Type         | Immediately        |
            | FollowUpType | Custom Cleared     |
            | Owner        | Salesman           |
            | Date         | Main Carriage ATD  |
            | Notes        | FU Creation result |
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

    Scenario: Update shipment to verify the automation exection
        Given the user fill "100" as ValueOfGoods and "MTA" as a MoveType
        When update shipment
        Then the direct should update successfully
        And the automation with follow up creation result should executed successfully

    Scenario: Edit an automation
        Given the user logged in
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: FU Creation when no condition of the Any conditions is fit
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area | Entity   | Type    | Field                   | Operator |
            | Or   | Shipment | boolean | Includes Customs        | IsEmpty  |
            | Or   | Shipment | boolean | Is Operationally Closed | IsEmpty  |
        And the user adds follow up creation result
            | Type         | Immediately        |
            | FollowUpType | Custom Cleared     |
            | Owner        | Salesman           |
            | Date         | Main Carriage ATD  |
            | Notes        | FU Creation result |
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

    Scenario: Update shipment to verify the automation exection
        Given the user fill "100" as ValueOfGoods and "MTA" as a MoveType
        When update shipment
        Then the direct should update successfully
        And the automation shouldn't executed

    Scenario: Edit an automation
        Given the user logged in
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: FU Creation when one of the All conditions is not fit
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area | Entity   | Type    | Field                   | Operator   |
            | And  | Shipment | boolean | Is Operationally Closed | IsnotEmpty |
            | And  | Shipment | boolean | Includes Customs        | IsEmpty    |
        And the user adds follow up creation result
            | Type         | Immediately        |
            | FollowUpType | Custom Cleared     |
            | Owner        | Salesman           |
            | Date         | Main Carriage ATD  |
            | Notes        | FU Creation result |
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

    Scenario: Update shipment to verify the automation exection
        Given the user fill "100" as ValueOfGoods and "MTA" as a MoveType
        When update shipment
        Then the direct should update successfully
        And the automation shouldn't executed

    Scenario: Edit an automation
        Given the user logged in
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: Create new automation OnCreate with FU Creation result
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnCreate" with automation detailes as following
            | Name        | Master Automation  |
            | Description | Master Description |
        And the user add the following condition
            | Area | Entity   | Type | Field                     | Operator | OperatorValue | ConditionValue |
            | Or   | Shipment | date | Actual Final Arrival Date | equals   | todayPlus     | 5              |
            | Or   | Master   | date | Main Carriage ATD         | equals   | todayPlus     | 5              |
        And the user adds follow up creation result
            | Type         | Immediately        |
            | FollowUpType | Custom Cleared     |
            | Owner        | Salesman           |
            | Date         | Main Carriage ATD  |
            | Notes        | FU Creation result |
        When create automation
        Then the new automation should create successfully

    Scenario: Edit an automation
        Given the user logged in
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnCreate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

