Feature: AgentComputed in house and master
    The user creates a Master Export Air shipment, creates a House Export Air shipment from withing the Master shipment,
    update the agent of master shipment to compute the AgentComputed, update the agent of house shipment to compute the AgentComputed.

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
        And the house computedAgent should be TestAgent 

     Scenario: Update Partners
        Given the user in the houses's Partners tab
        And the user add partner with following details
           | Agent                | TestAgent1   |
        When update house
        Then the master should update successfully
        And the house computedAgent should be TestAgent1 
