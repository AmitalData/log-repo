@release @all @stable
Feature: Split shipment
    The user creates a shipment, adds two containers,
    splits the shipment by moving one of the containers to a new shipment,
    checks to make sure that the new shipment has the moved container and checks the original shipment to
    make sure it has the remaining container.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Ocean             |
            | ShipmentType         | FCL               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
        When create shipment
        Then the direct should create successfully

    Scenario: Add Containers
        Given the user open the shipment and navigate to packages workspace
        Given  a container with the following details
            | PackageType | ContainerNumber | GrossWeight |
            | Bulk        | ABCD1234560     | 300.000     |
            | Flat Rack   | PQRS1875433     | 900.000     |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Split Shipment
        When split the shipment with the second container
        Then the direct shipment should split successfully

