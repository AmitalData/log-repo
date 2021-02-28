@release @all
Feature: Containers Follow-Up

    Feature Description

    Scenario: Create import ocean FCL shipment
        Given the user logged in and navigates to shipments workspace
        And a shipment with the following details
            | ShipmentLevel | Direction | TransportMode | ShipmentType | Consignee           | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Import    | Ocean         | FCL          | TestConsigneeImport | LHR                  | MIA                |
        When create shipment
        Then the shipment should create successfully

    Scenario: Add a container
        Given the user add a container with the following details
            | PackageType | ContainerNumber | GrossWeight |
            | PC2         | Random          | 100         |
        When update shipment
        Then the shipment should update successfully
        And it's status is "Order"

    Scenario: Add main carriage ATD
        Given edit main carriage leg ATD to "Today" at "12:00"
        When update shipment
        Then the shipment should update successfully
        And it's status is "Departed"
        And the container should appear in the "In Transit" view

    Scenario: Add main carriage ATA
        Given edit main carriage leg ATA to "Today" at "13:00"
        When update shipment
        Then the shipment should update successfully
        And it's status is "Arrived"
        And the container should not appear in the "In Transit" view