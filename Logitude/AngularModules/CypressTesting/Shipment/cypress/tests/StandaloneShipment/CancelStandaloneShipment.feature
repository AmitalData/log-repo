@standalone @devrelease
Feature: Cancel standalone shipment
    The user creates a Direct Import Ocean FCL shipment, from Pickup Create standalone shipment,Cancel standalone shipment
    Reactivate standalone shipment

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

    Scenario: Create standalone shipment when the pickup is FullResponsibility and from partner to casual address with same countries
        Given the user open the shipment and navigate to RoutingsTab workspace
        And add a new pickup leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Cancel standalone shipment
        Given the user in standalone shipment
        When cancel the standalone shipment with "Cancel Thestandalone shipment" Note
        Then the shipment should cancel successfully
        And the shipment should not connected with pickup
        And all fiellds in pickup should not be dim