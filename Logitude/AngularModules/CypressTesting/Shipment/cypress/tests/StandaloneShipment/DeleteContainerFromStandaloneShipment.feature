@Devstandalone 
Feature: Delete container from standalone shipment
    The user creates a Direct Import Ocean FCL shipment, add packages,
    Create standalone shipment,Delete the container from standalone shipment

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
            | PackageType | ContainerNumber | GrossWeight |
            | Bulk        | ABCD1234560     | 100         |
        When save shipment
        Then the direct shipment should save successfully

    Scenario:  FullResponsibility and "To/From" are partners with same countries
        Given navigate to RoutingsTab workspace
        And add a new pickup leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        And the user in the pickup packages select the container
        And save the pickup
        When create standalone shipment
        Then a domestic inland shipment should create
        And the cancel, operational close Shipment, convert to custom file and Send Response actions in more button shouldn't be dim
        And all other actions should be dim
        And all fields should be dim in pickup window
        And the link of standalon should display

    Scenario: Delete the container from standalone shipment
        Given the user in the standalone shipment Packages tab
        And click delete button
        When save Standalone shipment
        Then the direct shipment should save successfully
        And  the container delete from pickup leg packages tab
        And in forwarder shipment the container appear
