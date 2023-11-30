@release @stable
Feature: Containers Delivery
    The user creates a Direct Import Ocean FCL shipment, adds a container, modifies the dates of the shipment,
    adds deliveries for the container that will show as legs in the Routings tab and reflect the different stages of In Transit,
    Arrived Not Delivered and Delivered Not Returned.

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
        Given edit main carriage leg ATD to "Today" at "10:00"
        When update shipment
        Then the shipment should update successfully
        And it's status is "Departed"
        And the container should appear in the "In Transit" view

    Scenario: Add main carriage ATA
        Given edit main carriage leg ATA to "Today" at "11:00"
        When update shipment
        Then the shipment should update successfully
        And it's status is "Arrived"
        And the container should not appear in the "In Transit" view

    Scenario: Add delivery container delivery
        Given the user add a delivery container delivery with "Today" at "12:00" as actual departure
        When update "new" container delivery
        Then the "new" container delivery should update successfully
        And the container should appear in the "Arrived Not Delivered" view

    Scenario: Edit delivery container delivery
        Given the user edit a delivery container delivery with "Today" at "13:00" as actual arrival
        When update "Editted" container delivery
        Then the "Editted" container delivery should update successfully
        And the container should not appear in the "Arrived Not Delivered" view
        And a new "Delivery" leg will appear in the Routings tab

    Scenario: Add empty container return container delivery
        Given the user add an empty container return container delivery with "Today" at "14:00" as actual departure
        When update "new" container delivery
        Then the "new" container delivery should update successfully
        And the container should appear in the "Delivered Not Returned" view

    Scenario: Edit empty container return container delivery
        Given the user edit an empty container return container delivery with "Today" at "15:00" as actual arrival
        When update "Editted" container delivery
        Then the "Editted" container delivery should update successfully
        And the container should not appear in the "Delivered Not Returned" view
        And a new "Empty Container Return" leg will appear in the Routings tab

