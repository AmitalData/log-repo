@standalone @devrelease
Feature: Update Standalone shipment
    The user creates a Direct Import Ocean FCL shipment,Create standalone shipment from pickup
    Update routing tab in standalone shipment

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

    Scenario: Create standalone shipment when pickup from partner to port with the same countries
        Given the user open the shipment and navigate to RoutingsTab workspace
        And add a new pickup leg with the following details
            | FullResponsibility | True                      |
            | From               | Partner                   |
            | FromPartner        | ALS CUSTOMS SERVICES GMBH |
            | To                 | Port                      |
            | ToPort             | Brandscheid/Westerwald    |
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Update routing tab in standalone shipment
        Given the user in the standalone shipment routings tab
        And edit edit main carriage leg with the following details
            | Trucker       | Testing trucker |
            | Driver        | test            |
            | TruckNumber   | 1234            |
            | TrailerNumber | 45678           |
            | ETD           | 06/01/2021      |
            | ETA           | 06/02/2021      |
            | ATD           | 06/03/2021      |
            | ATA           | 06/10/2021      |
        When update shipment
        Then the shipment should update successfully
        And the pickup window should update successfully with the following details
            | Trucker       | AJPES      |
            | Driver        | test       |
            | TruckNumber   | 1234       |
            | TrailerNumber | 45678      |
            | ETD           | 06/01/2021 |
            | ETA           | 06/02/2021 |
            | ATD           | 06/03/2021 |
            | ATA           | 06/10/2021 |
