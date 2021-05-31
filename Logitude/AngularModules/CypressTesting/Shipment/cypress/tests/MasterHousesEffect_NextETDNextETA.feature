Feature: Compute NextETA in Master And Connected Houses
    The user creates a Master Export Air shipment, creates a House Export Air shipment from withing the Master shipment,
    Compute FinalArrivalDate in Master And Connected Houses.

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

     Scenario: Update routing tab with main carriage estimated dates 
        Given the user in the master's rounting tab
        And edit main carriage leg with the following details
              | MainCarriageETD      | 2021-05-02 |
              | MainCarriageETA      | 2021-05-04 |
        When update master
        Then the master should update successfully
        And the master NextETD should be 2021-05-02
        And the master NextETA should be 2021-05-04
        And the house NextETD should be 2021-05-02
        And the house NextETA should be 2021-05-04

     Scenario: Update routing tab with main carriage Actual dates 
        Given the user in the master's rounting tab
        And edit main carriage leg with the following details
              | MainCarriageETD      | 2021-05-02 |
              | MainCarriageETA      | 2021-05-04 |
              | MainCarriageATD      | 2021-05-05 |
              | MainCarriageATA      | 2021-05-06 |
        When update master
        Then the master should update successfully
        And the master NextETD should be null
        And the master NextETA should be null
        And the house NextETD should be null
        And the house NextETA should be null

     Scenario: Update routing tab with Transshipment1 Estimated dates 
        Given the user in the master's rounting tab
        And edit Transshipment1 leg with the following details
            | Transshipment1FromPort     | TLV        |
            | Transshipment1ETD          | 2021-05-06 |
            | Transshipment1ETA          | 2021-05-07 |
        When update master
        Then the master should update successfully
        And the master NextETD should be 2021-05-06 
        And the master NextETA should be 2021-05-07
        And the house NextETD should be 2021-05-06 
        And the house NextETA should be 2021-05-07


