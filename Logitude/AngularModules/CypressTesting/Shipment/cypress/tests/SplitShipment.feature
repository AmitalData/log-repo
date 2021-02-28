@smoke @not-stable @all
Feature: Split shipment
    After the user logging in the system and navigate to shipments workspace
    will create a direct shipment , Add Containers then split it.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | ShipmentType | Shipper           | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Ocean         | FCL          | TestShipperExport | LHR                  | MIA                |
        When create shipment
        Then the direct should create successfully

    Scenario: Add Containers
        Given the user open the shipment and navigate to packages workspace
        Given  a container with the following details
            | PackageType | PackageNumber | GrossWeight |
            | Bulk        | ABCD1234560   | 300.000     |
            | Flat Rack   | PQRS1875433   | 900.000     |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Split Shipment
        When split the shipment with the second container
        Then the direct shipment should split successfully

