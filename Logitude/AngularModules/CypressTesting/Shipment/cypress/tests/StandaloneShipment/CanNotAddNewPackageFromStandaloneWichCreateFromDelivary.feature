@standalone @devrelease
Feature: The User Can't add new packages in standalone shipment which created from delivery
    Create import ocean FCL shipment, Add Delivery,
    the user can't add new packages in standalone shipment

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

    Scenario: Create standalone shipment when the Delivery is FullResponsibility and from partner to partner with same countries
        Given the user open the shipment and navigate to RoutingsTab workspace
        And add a new Delivery leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        And save the Delivery
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in Delivery window
        And the link of standalon should display

    Scenario: The User can't add new packages from standalone shipment which created from delivery
        Given the user in the standalone shipment Packages tab
        When click add full truack container
        Then the Blus button disappear in window