Feature:  Can't create standalon from pickup which has many containers
    The user creates a Direct Import Ocean FCL shipment, add packages,
    Add pickup and select all container


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

    Scenario: Add Packages
        Given the user open the shipment and navigate to packages workspace
        And  a container with the following details
            | PackageType        | ContainerNumber | GrossWeight |
            | Bulk               | ABCD1234560     | 100         |
            | Flat Rack          | ABCD1234770     | 200         |
            | Standard Container | ABCD1234889     | 400         |
        When save shipment
        Then the direct shipment should save successfully


    Scenario: Add pickup and select all container
        Given the user in shipment routing tab
        And add a new pickup leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        And the user in the pickup packages select all container
        When click create standalone shipment
        Then A validation message should appear Can't Create a Stand Alone Shipment Since Pickup has more than one Container


