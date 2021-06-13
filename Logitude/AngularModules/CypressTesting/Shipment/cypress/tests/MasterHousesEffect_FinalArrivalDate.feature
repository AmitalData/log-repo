Feature: Compute FinalArrivalDate in Master And Connected Houses
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

    Scenario: Update FinalArrivalDate when there is actual date
        Given the user in the master rounting tab
        And edit main carriage leg with the following details
            | MainCarriageETADate | 2021-05-02 |
            | MainCarriageATADate | 2021-05-04 |
        When update master
        Then the master should update successfully
        And the master FinalArrivalDate should be 2021-05-04
        And the house FinalArrivalDate should be 2021-05-04

    Scenario: Update FinalArrivalDate when there is no actual date
        Given the user in the master rounting tab
        And edit main carriage leg with the following details
            | MainCarriageETADate | 2021-05-02 |
            | MainCarriageATADate | 0          |
        When update master
        Then the master should update successfully
        And the master FinalArrivalDate should be 2021-05-02
        And the house FinalArrivalDate should be 2021-05-02

    Scenario: Update FinalArrivalDate when there are transshipments
        Given the user in the master's rounting tab
        And edit main carriage leg with the following details
            | Transshipment1FromPortId | TLV        |
            | Transshipment1ETA        | 2021-05-04 |
            | Transshipment1ATA        | 2021-05-05 |
            | MainCarriageETADate      | 2021-05-02 |
            | MainCarriageATADate      | 2021-05-03 |
        When update master
        Then the master should update successfully
        And the master FinalArrivalDate should be 2021-05-05
        And the house FinalArrivalDate should be 2021-05-05