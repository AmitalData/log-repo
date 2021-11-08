
Feature: Status in Routings Tab
    The user creates a direct export air shipment, direct import ocean shipment,
    direct domestic inland shipment and direct Drop Air shipment.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the direct should create successfully

    Scenario: Update routing tab
        Given the user open the shipment and navigate to RoutingsTab workspace
        And add a warehouse with "TestWarehouse" as a terminal
        And the user add new pickup
        And add delivery with "TestAgent" as a partner routing
        And the status value should be "Order"

    Scenario: Add Expected Departure date for the pickup
        Given the user edit pick expected departure with '01/10/2021' as a value
        When save pickup
        Then the direct should update successfully
        And The status of the shipment  equal Pick Up Arranged

    Scenario: Add Actual Departure date for the pickup
        Given the user edit pickup with the following details
            | Actual Departure | 02/10/2021 |
        When save pickup
        Then the direct should update successfully
        And The status of the shipment  equal Pick Up

    Scenario: Add Actual Arrival date for the pickup
        Given the user edit pickup with the following details
            | Actual Arrival | 03/10/2021 |
        When save pickup
        Then the direct should update successfully
        And The status of the shipment equal On Hand(LHR)



    Scenario: Add Actual Entry date for the Warehouse
        Given the user edit Warehouse with the following details
            | Actual Entry | 04/10/2021 |
        When save Warehouse
        Then the direct should update successfully
        And The status of the shipment equal Storage Entry(TestWarehouse)

    Scenario: Add Actual Release date for the Warehouse
        Given the user edit Warehouse with the following details
            | Actual Release | 05/10/2021 |
        When save Warehouse
        Then the direct should update successfully
        And The status of the shipment equal Storage Released(TestWarehouse)


    Scenario:Edit main carriage Add ATD
        Given the user Edit main carriage  with the following details
            | ATD | 06/10/2021 |
        When  click save
        Then the direct should update successfully
        And The status of the shipment equal Departed(MIA)


    Scenario:Edit main carriage Add ATA
        Given the user Edit main carriage  with the following details
            | ATA | 07/10/2021 |
        When  click save
        Then the direct should update successfully
        And The status of the shipment equal Departed(MIA)



    Scenario: Add Expected Departure date for the Delivary
        Given the user edit Delivary with the following details
            | Expected Departure | 08/10/2021 |
        When save Delivary
        Then the direct should update successfully
        And The status of the shipment  equal Delivery Arranged(MIA)

    Scenario: Add Actual Departure date for the Delivary
        Given the user edit Delivary with the following details
            | Actual Departure | 10/10/2021 |
        When save Delivary
        Then the direct should update successfully
        And The status of the shipment  equal Delivery(MIA)


    Scenario: Add Actual Arrival date for the Delivary
        Given the user edit Delivary with the following details
            | Actual Arrival | 12/10/2021 |
        When save Delivary
        Then the direct should update successfully
        And The status of the shipment equal Delivered

















