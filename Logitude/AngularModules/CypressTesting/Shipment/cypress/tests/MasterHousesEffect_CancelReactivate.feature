Feature: Cancel And Reactivate Master And Connected House
    The user creates a Master Export Air shipment, creates a House Export Air shipment from withing the Master shipment,
    cancel the master and house, reactivate them.

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

    Scenario: Cancel the master shipment
        When cancel the master shipment with "Cancel TheShipment" Note
        Then the house should Cancel successfully


    Scenario: Reactivate the master shipment
        When reactivate the shipment with "Reactivate The Shipment" Note
        Then the house should Reactivate successfully