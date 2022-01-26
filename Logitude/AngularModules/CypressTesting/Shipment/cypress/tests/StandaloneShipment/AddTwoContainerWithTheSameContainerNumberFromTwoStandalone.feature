@standalone @devrelease
Feature: Add Two Container with the same container number from two standalone shipment
    The user creates a Direct Import Ocean FCL shipment,Create standalone shipment from first pickup,
    Add Packages from standalone,Create standalone shipment from second pickup,Add Packages from standalone


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

    Scenario: Create standalone shipment when the first pickup is FullResponsibility and from partner to partner with same countries
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

    Scenario: Add Package from the first standalone
        Given the user in the standalone shipment Packages tab
        And a container with the following details
            | PackageType | ContainerNumber | GrossWeight |
            | Bulk        | ABCD1234560     | 100         |
        When save shipment
        Then the direct shipment should save successfully
        And The container appear in the pickup


    Scenario: Create standalone shipment when the second  pickup is FullResponsibility and from partner to partner with same countries
        Given the user in the shipment's routong tab
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

    Scenario: Add Package from standalone
        Given the user in the standalone shipment Packages tab
        And a container with the following details
            | PackageType | ContainerNumber | GrossWeight |
            | Bulk        | ABCD1234560     | 100         |
        When save shipment
        Then a validation message with "Cannot have 2 containers with same number" error should appear

