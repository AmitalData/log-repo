Feature: Create Standalone Shipment from Routing
    The user creates a Direct Import Ocean FCL shipment, create standalone shipment from pickup,create standalone shipment  from pickup child,
    create standalone shipment from routing

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

    Scenario: Create standalone shipment from routing in pickup screen
        Given the user open the shipment and navigate to RoutingsTab workspace
        And add Standalone Shipment With Pickup leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        When create shipment
        Then a domestic inland shipment should create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Create standalone shipment from routing in Delivary screen
        Given the user in the shipment routings tab
        And add Standalone Shipment With delivary leg with the following details
            | Shipper  | TestShipperExport      |
            | From     | Port                   |
            | FromName | Brandscheid/Westerwald |
            | To       | Casual Address         |
            | Country  | Germany                |
            | City     | test                   |
        When create shipment
        Then a domestic inland shipment should create
        And delivary leg will create
        And the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display

