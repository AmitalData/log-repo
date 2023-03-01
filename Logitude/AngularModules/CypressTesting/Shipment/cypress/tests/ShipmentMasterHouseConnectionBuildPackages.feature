#@release
@devrelease
Feature: Shipment Master House Connection Build Packages
    The user creates a Master Export Air shipment, creates a House Export Air shipment from within the Master shipment,
    build packages from inside the house shipment (include air case(packages), ocean/inland case (containers + packages))
    after that generate packages from the master.

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

    Scenario: Add Packages to house shipment
        Given the user navigate to the packages workspace
        And the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 12       | 100    | 100   | 100    | 1200        |
            | 100      | 200    | 200   | 200    | 2400        |
        When save shipment
        Then the shipment should save successfully

    Scenario: generate the house shipment Packages in the master shipment
        Given the user navigate to packages workspace in master shipment
        And bress on build from shipments
        When save the shipment
        Then the shipment should save successfully
        And the shipment should have two packages

    Scenario: Create master export Ocean shipment
        Given the user navigates to shipments workspace
        And a master Shipment with following details
            | ShipmentLevel        | Master |
            | Direction            | Export |
            | TransportMode        | Ocean  |
            | ShipmentType         | FCL    |
            | Agent                | Test   |
            | MainCarriageFromPort | LHR    |
            | MainCarriageToPort   | MIA    |
        When create shipment
        Then the master should create successfully

    Scenario: Create house export ocean shipment inside the master
        Given the user in the master's Shipment tab
        When create house with "TestShipperExport" as Shipper
        Then the house should create successfully
        And the house should connect successfully

    Scenario: Add Containers
        Given the user navigate to the packages workspace
        And the following details
            | PackageType | ContainerNumber | GrossWeight |
            | Bulk        | ABCD1234560     | 300.000     |
            | Flat Rack   | PQRS1875433     | 900.000     |
        When save shipment
        Then the shipment should save successfully

    Scenario: generate the house shipment container in the master shipment
        Given the user navigate to packages workspace in master shipment
        And bress on build from shipments
        When save the shipment
        Then the shipment should save successfully
        And the shipment should have two packages