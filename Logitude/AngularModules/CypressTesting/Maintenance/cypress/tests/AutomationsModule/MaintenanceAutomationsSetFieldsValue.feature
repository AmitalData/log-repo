@devrelease
Feature: Automations Creation and Updating for Set Fields Value Result
    The user creates Automations on update and On Create ,and edit it from the Automations Module,and test the automation from shipment side.

    Scenario: Create new automation OnUpdate with set field value result and conditions are fit(All conditions)
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
        And the use adds Set Fields Value result
            | Field         | Customs Clearance Date |
            | Fieldtype     | date                   |
            | Operator      | Set Constant Value     |
            | Operatorvalue | date                   |
            | Value         | .                      |
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
        And the Automation should executed successfully

    Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: Create new automation OnUpdate with set field value result and conditions are fit(ANY conditions)
        Given the user logged in
        And the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area | Entity   | Type    | Field                   | Operator   |
            | Or   | Shipment | boolean | Is Operationally Closed | IsnotEmpty |
            | Or   | Shipment | boolean | Includes Customs        | IsnotEmpty |
        And the use adds Set Fields Value result
            | Field         | Customs Clearance Date |
            | Fieldtype     | date                   |
            | Operator      | Set Constant Value     |
            | Operatorvalue | Date                   |
            | Value         | .                      |
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
        And the Automation should executed successfully

    Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: Create new automation OnUpdate with set field value result and one of the ANY conditions is not fit
        Given the user logged in
        And the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area | Entity   | Type    | Field                   | Operator   |
            | Or   | Shipment | boolean | Is Operationally Closed | IsnotEmpty |
            | Or   | Shipment | boolean | Includes Customs        | IsEmpty    |
        And the use adds Set Fields Value result
            | Field         | Customs Clearance Date |
            | Fieldtype     | date                   |
            | Operator      | Set Constant Value     |
            | Operatorvalue | Date                   |
            | Value         | .                      |
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
        And the Automation should executed successfully

    Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: Create new automation OnUpdate with set field value result and no condition of the Any conditions is fit
        Given the user logged in
        And the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area | Entity   | Type    | Field                   | Operator |
            | Or   | Shipment | boolean | Includes Customs        | IsEmpty  |
            | Or   | Shipment | boolean | Is Operationally Closed | IsEmpty  |
        And the use adds Set Fields Value result
            | Field         | Customs Clearance Date |
            | Fieldtype     | date                   |
            | Operator      | Set Constant Value     |
            | Operatorvalue | Date                   |
            | Value         | .                      |
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
        And the Automation shouldn't executed

    Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Shipments" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully

    Scenario: Create new automation OnUpdate with set field value result and one of the All conditions is not fit
        Given the user logged in
        And the user navigates to automations menu
        And the user select "Shipments" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Shipments Automation  |
            | Description | Shipments Description |
        And the user add the following condition
            | Area | Entity   | Type    | Field                   | Operator   |
            | And  | Shipment | boolean | Is Operationally Closed | IsnotEmpty |
            | And  | Shipment | boolean | Includes Customs        | IsEmpty    |
        And the use adds Set Fields Value result
            | Field         | Customs Clearance Date |
            | Fieldtype     | date                   |
            | Operator      | Set Constant Value     |
            | Operatorvalue | Date                   |
            | Value         | .                      |
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
        And the Automation shouldn't executed

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
            | Area | Entity | Type | Field             | Operator | OperatorValue | ConditionValue |
            | And  | Master | date | Main Carriage ATD | equals   | todayPlus     | 5              |
            | And  | Master | date | Main Carriage ATA | equals   | todayPlus     | 5              |
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

    # Tickets
    Scenario: Create new automation OnUpdate with set field value result
        Given the user logged in
        Given the user navigates to automations menu
        And the user select "Tickets" as entity
        And the user add automation "OnUpdate" with automation detailes as following
            | Name        | Tickets Automation  |
            | Description | Tickets Description |
        And the user add the following condition
            | Area | Entity | Type | Field               | Operator | OperatorValue | ConditionValue |
            | And  | Ticket | date | First Resolve Date  | equals   | todayPlus     | 5              |
            | And  | Ticket | date | First Response Time | equals   | todayPlus     | 5              |
        And the use adds Set Fields Value result
            | Field         | First Resolve Date |
            | Fieldtype     | date               |
            | Operator      | Set Constant Value |
            | Operatorvalue | Date               |
            | Value         | .                  |
        When create automation
        Then the new automation should create successfully

    Scenario: Edit an automation
        Given the user navigates to automations menu
        And the user select "Tickets" as entity
        And choose the first automation from "OnUpdate"
        And inactive the automation
        When update automation
        Then the automation should updated successfully