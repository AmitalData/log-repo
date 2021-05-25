Feature: Shipment Master House Connection
    The user creates a Master Export Air shipment, creates a House Export Air shipment from withing the Master shipment,
    disconnects the House from the Master, creates a House Export Air shipment from outside the Master,
    connects the House to the Master from within the Master shipment and then disconnects the House shipment.

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

    Scenario: Create house export air shipment inside the master
        Given the user in the master's Shipment tab
        When create house with "TestShipperExport" as Shipper
        Then the house should create successfully
        And the house should connect successfully

    Scenario: Disconnect the house shipment
        When disconnect shipment
        Then the shipment should disconnect successfully

    Scenario: Create house export air shipment
        Given a house Shipment with the following details
            | ShipmentLevel        | House             |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create house shipment
        Then the house should create successfully

    Scenario: Connect the house shipment to the master
        Given the user in the master's Shipment tab
        When connect the house shipment
        Then the shipment should connect successfully

    Scenario: Disconnect the house shipment
        When the user disconnect the house shipment
        Then the shipment should disconnect successfully