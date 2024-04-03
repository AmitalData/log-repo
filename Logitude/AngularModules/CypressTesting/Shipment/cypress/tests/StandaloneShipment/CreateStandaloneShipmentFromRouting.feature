@Devstandalone 
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

    Scenario: Create standalone shipment from routing With pickup From Partner to CasualAddress
        Given the user open the shipment and navigate to RoutingsTab workspace
        And add Standalone Shipment With Pickup leg with the following details
            | From        | Partner             |
            | FromPartner | TestConsigneeImport |
            | To          | CasualAddress       |
            | ToCountry   | State Of Palestine  |
            | ToCity      | Bethlehem           |
        When create shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And the pickup will add all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Create standalone shipment from routing With pickup From CasualAddress TO port
        Given the user in the shipment's routings tab add standalone with pickup
        And add Standalone Shipment With Pickup leg with the following details
            | From        | CasualAddress      |
            | FromCountry | State Of Palestine |
            | FromCity    | Ramallah           |
            | To          | Port               |
            | ToPort      | Palestina          |
        When create shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And the pickup will add all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Create standalone shipment from routing With pickup From port to port
        Given the user in the shipment's routings tab add standalone with pickup
        And add Standalone Shipment With Pickup leg with the following details
            | From     | Port                   |
            | FromPort | Brandscheid/Westerwald |
            | To       | Port                   |
            | ToPort   | Brandshagen            |
        When create shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And the pickup will add all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Create standalone shipment from routing  With Delivary from port to port
        Given the user in the shipment's routings tab
        And add Standalone Shipment With delivery leg with the following details
            | From     | Port                   |
            | FromPort | Brandscheid/Westerwald |
            | To       | Port                   |
            | ToPort   | Brandshagen            |
        When create shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And the delivery will add all fields should be dim in delivery window
        And the link of standalon should display


    Scenario: Create standalone shipment from routing With Delivary from Partner to partner
        Given the user in the shipment's routings tab
        And add Standalone Shipment With delivery leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        When create shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And the delivery will add all fields should be dim in delivery window
        And the link of standalon should display

    Scenario: Create standalone shipment from routing With Delivary from CasualAddress to CasualAddress
        Given the user in the shipment's routings tab
        And add Standalone Shipment With delivery leg with the following details
            | From        | CasualAddress      |
            | FromCountry | State Of Palestine |
            | FromCity    | Ramallah           |
            | To          | CasualAddress      |
            | ToCountry   | State Of Palestine |
            | ToCity      | Bethlehem          |
        When create shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And the delivery will add all fields should be dim in delivery window
        And the link of standalon should display

