@Release @all
Feature: Status in Routings Tab
    The user creates a direct export air shipment, update routing tab,Add Expected Departure date for the pickup,
    add Actual Departure date for the pickup,add Actual Arrival date for the pickup,
    add Actual Entry date for the Warehouse,add Actual Release date for the Warehouse,
    edit main carriage Add ATD, edit main carriage Add ATA,Add Expected Departure date for the Delivary,
    add Actual Departure date for the Delivary,add Actual Arrival date for the Delivary

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
        And add delivery with "TestAgent" as a partner routing
        And the user add new pickup
        And the status value should be "Order"

    Scenario: Add Expected Departure date for the pickup
        Given the user edit pickup window
        And add expected departure with '01/10/2020' as a value and Notes 'expected departure equal 01/10/2020'
        When save pickup
        Then the direct should update successfully
        And the status value should be "Pick Up Arranged"

    Scenario: Add Actual Departure date for the pickup
        Given the user edit pickup window
        And add Actual Departure with '02/10/2021' as a value and Notes 'actual departure equal 02/10/2020'
        When save pickup
        Then the direct should update successfully
        And the status value should be "Pick Up"

    Scenario: Add Actual Arrival date for the pickup
        Given the user edit pickup window
        And add Actual Arrival with '03/10/2021' as a value and Notes 'actual arrival equal 03/10/2020'
        When save pickup
        Then the direct should update successfully
        And the status value should be "On Hand"

    Scenario: Add Actual Entry date for the Warehouse
        Given the user edit Warehouse window
        And add Actual Entry with '04/10/2021' as a value
        When save Warehouse
        Then the direct should update successfully
        And the status value should be "Storage Entry"

    Scenario: Add Actual Release date for the Warehouse
        Given the user edit Warehouse window
        And add Actual Release with '05/10/2021' as a value
        When save Warehouse
        Then the direct should update successfully
        And the status value should be "Storage Released"

    Scenario: Edit main carriage Add ATD
        Given the user edit main carriage window
        And add ATD with '06/10/2021' as a value
        When save main carriage
        Then the direct should update successfully
        And the status value should be "Departed"

    Scenario: Edit main carriage Add ATA
        Given the user edit main carriage window
        And add ATA with '07/10/2021' as a value
        When save main carriage
        Then the direct should update successfully
        And the status value should be "Departed"

    Scenario: Add Expected Departure date for the Delivary
        Given the user edit Delivary window
        And add expected departure with '08/10/2021' as a value and Notes 'expected departure equal 08/10/2020'
        When save Delivary
        Then the direct should update successfully
        And the status value should be "Delivery Arranged"

    Scenario: Add Actual Departure date for the Delivary
        Given the user edit Delivary window
        And add Actual Departure with '10/10/2021' as a value and Notes 'actual departure equal 10/10/2020'
        When save Delivary
        Then the direct should update successfully
        And the status value should be "Delivery"

    Scenario: Add Actual Arrival date for the Delivary
        Given the user edit Delivary window
        And add Actual Arrival with '12/10/2021' as a value and Notes 'actual arrival equal 12/10/2020'
        When save Delivary
        Then the direct should update successfully
        And the status value should be "Delivered"
















