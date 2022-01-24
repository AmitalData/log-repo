Feature: Add Container without container number
    The user creates a Direct Import Ocean FCL shipment, add packages,add pickup

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

    Scenario: Add packages
        Given the user open the shipment and navigate to packages workspace
        And  a container with the following details
            | PackageType | GrossWeight |
            | Bulk        | 100         |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Add pickup and select all container
        Given the user in shipment routing tab
        And add a new pickup leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        When the user in the pickup packages
        Then no container will appear in the window