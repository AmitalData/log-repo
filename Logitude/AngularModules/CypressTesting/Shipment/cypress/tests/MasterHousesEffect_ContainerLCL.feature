Feature: LCL Container Number
    The user creates a master Import Ocean groupage shipment,
    creates house, connects the house with the master , creates package with container number in the house,
    rebuild master's container with the house package with diff container number, changes the house conatianer number.

    Scenario: Create import ocean FCL master
        Given the user logged in and navigates to shipments workspace
        And a shipment with the following details
            | ShipmentLevel        | Master    |
            | Direction            | Import    |
            | TransportMode        | Ocean     |
            | ShipmentType         | Groupage  |
            | Agent                | TestAgent |
            | MainCarriageFromPort | LHR       |
            | MainCarriageToPort   | MIA       |
        When create shipment
        Then the shipment should create successfully

    Scenario: Create house import ocean LCL shipment inside the master
        Given the user in the master's Shipment tab
        When create house with "TestShipperExport" as Shipper
        Then the house should create successfully
        And the house should connect successfully

    Scenario: Add a package in the house
        Given the user in the house package tab
        And add a package with the following details
            | PackageType | ContainerNumber | Pieces | GrossWeight |
            | AN          | ABCD1111117     | 5      | 100         |
        When update shipment
        Then the shipment should update successfully
        And the package should add successfully

    Scenario: Rebuild master packages in the master
        Given the user in the master package tab
        And rebuild master containers by adding new container with the following details
            | PackageType | GrossWeight |
            | PC2         | 100         |
        When Update the shipment
        Then the shipment should update successfully
        And the container should add successfully
        And the master container number should be ABCD1111117

    Scenario: Change container number from master
        Given the user in the master package tab
        And edit the continer number in master shipment 
            | ContainerNumber |
            | DDDD88889       |
        When update shipment
        Then the shipment should update successfully
        And the container should change successfully
        And the house container number should be DDDD88889
