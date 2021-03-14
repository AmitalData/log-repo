@release @dev
Feature: Containers Follow-Up

    The user creates a Direct Import Ocean FCL shipment,
    adds a container, modifies the dates of the shipment, adds follow-ups for the
    container to reflect the different stages of In Transit, Arrived Not Delivered and Delivered Not Returned.

    Scenario: Create import ocean FCL shipment
        Given the user logged in and navigates to shipments workspace
        And a shipment with the following details
            | ShipmentLevel        | Direct              |
            | Direction            | Import              |
            | TransportMode        | Ocean               |
            | ShipmentType         | FCL                 |
            | Consignee            | TestConsigneeImport |
            | MainCarriageFromPort | LHR                 |
            | MainCarriageToPort   | MIA                 |
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

    Scenario: Add delivery follow up
        Given the user add a delivery follow up with "Today" at "14:00" as actual departure
        When update follow up
        Then the follow up should update successfully
        And the container should appear in the "Arrived Not Delivered" view

    Scenario: Edit delivery follow up
        Given the user edit a delivery follow up with "Today" at "14:00" as actual arrival
        When update follow up
        Then the follow up should update successfully
        And the container should not appear in the "Arrived Not Delivered" view

    Scenario: Add empty container return follow up
        Given the user add an empty container return follow up with "Today" at "14:00" as actual departure
        When update follow up
        Then the follow up should update successfully
        And the container should appear in the "Delivered Not Returned" view

    Scenario: Edit empty container return follow up
        Given the user edit an empty container return follow up with "Today" at "14:00" as actual arrival
        When update follow up
        Then the follow up should update successfully
        And the container should not appear in the "Delivered Not Returned" view