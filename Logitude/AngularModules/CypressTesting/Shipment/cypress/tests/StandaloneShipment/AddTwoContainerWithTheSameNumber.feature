@standalone @devrelease
Feature: Add Two container with the same container number in forwarder shipment
    The user creates a Direct Import Ocean FCL shipment, Add first container,Add second container

    Scenario: Create import ocean FCL shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct              |
            | Direction            | Import              |
            | TransportMode        | Ocean               |
            | ShipmentType         | FCL                 |
            | Shipper              | Israeli Tenant      |
            | Consignee            | TestConsigneeImport |
            | MainCarriageFromPort | LHR                 |
            | MainCarriageToPort   | MIA                 |
        When create shipment
        Then the shipment should create successfully

    Scenario: Add Two container
        Given the user open the shipment and navigate to packages workspace
        And  a container with the following details
            | PackageType | ContainerNumber | GrossWeight |
            | Flat Rack   | 10              | 100         |
            | Bulk        | 10              | 100         |
        When save shipment
        Then a validation message with "Cannot have 2 containers with the same number, you can use inside packages to add detailed packages" error should appear




