#@release
@devrelease
Feature: Shipment Master House Connection Routing Tab
    The user creates a Master Export Air shipment, creates a House Export Air shipment from within the Master shipment.
    Add pre/on carriage on master and pre/on forwarding on house and assert that Pre/On carriage are dim in house level

    Scenario: Create master export air shipment
        Given the user logged in and navigates to shipments workspace
        And a master Shipment with following details
            | ShipmentLevel        | Master    |
            | Direction            | Export    |
            | TransportMode        | Air       |
            | Agent                | TestAgent |
            | MainCarriageFromPort | LHR       |
            | MainCarriageToPort   | MIA       |
        When create shipment
        Then the master should create successfully

    Scenario: Add pre carriage and on carriage on master shipment
        Given open the shipment
        And add pre carriage from port "JFK" to port "MIA"
        And add on carriage from port "JFK" to port "MIA"

    Scenario: Create house export air shipment inside the master
        Given the user in the master's Shipment tab
        When create house with "TestShipperExport" as Shipper
        Then the house should create successfully
        And the house should connect successfully

    Scenario: Add pre forwarding on house shipment
        Given navigates routing tab in house shipment
        And add pre forwarding from port "JFK"
        When update the shipment
        Then the shipment should update successfully

    Scenario: Add on forwarding on house shipment
        Given add on forwarding to port "MIA"
        When update the shipment
        Then the shipment should update successfully

    Scenario: Assert pre carriage is dim
        When open pre carriage edit screen
        Then this message "In house shipments, Pre Carriage should be edited from the master" should be printed

    Scenario: Assert on carriage is dim
        When open on carriage edit screen
        Then this message "In house shipments, On Carriage should be edited from the master" should be printed