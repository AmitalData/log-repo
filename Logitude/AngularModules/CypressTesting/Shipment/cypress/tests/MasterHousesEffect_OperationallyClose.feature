Feature: Close Master And Connected House Operationally 
    The user creates a Master Export Air shipment, creates a House Export Air shipment from withing the Master shipment,
    update the routing tab of master shipment to close it operationallay, close the master operationally 

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

     Scenario: Update routing tab
        Given the user in the master's rounting tab
        And edit main carriage leg with the following details
              | Airline      | AA     |
              | FlightNumber | Random |
              | MAWB         | Random |
              | ATD          | Today  |
        When update master
        Then the master should update successfully

    Scenario: Create house export air shipment inside the master
        Given the user in the master's Shipment tab
        When create house with "TestShipperExport" as Shipper
        Then the house should create successfully
        And the house should connect successfully

    Scenario: Close the master shipment Operationally 
        When close the master shipment Operationally
        Then the master should close operationally successfully
        And the connected house should close operationally successfully

    Scenario: Reopen master shipment operationally
        When reopen master operationally with "reopen operationally" Note
        Then the master should reopen successfully
        And the connected house should reopen successfully