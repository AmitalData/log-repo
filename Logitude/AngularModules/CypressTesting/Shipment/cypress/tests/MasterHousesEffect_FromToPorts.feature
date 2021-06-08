Feature: Update the routing tab of master shipment
    The user creates a Master Export Air shipment, creates a House Export Air shipment from withing the Master shipment,
    update the routing tab of master shipment and the connceted house 
    Scenario: Create master export air shipment
        Given the user logged in and navigates to shipments workspace
        And a master Shipment with following details
            | ShipmentLevel        | Master    |
            | Direction            | Export    |
            | TransportMode        | Air       |
            | Agent                | TestAgent |
            | MainCarriageFromPort | LHR       |
            | MainCarriageToPort   | MIA       |
        When create master
        Then the master should create successfully

    Scenario: Create house export air shipment inside the master
        Given the user in the master's Shipment tab
        When create house with "TestShipperExport" as Shipper
        Then the house should create successfully
        And the house should connect successfully

    Scenario: Update routing tab
        Given the user in the master's rounting tab
        And edit main carriage leg with the following details
            | Gateway     | FRA |
            | Destination | TLV |
        When update master
        Then the master should update successfully
        Then the connceted house main carriage leg should update with the following
            | Gateway     | FRA |
            | Destination | TLV |
