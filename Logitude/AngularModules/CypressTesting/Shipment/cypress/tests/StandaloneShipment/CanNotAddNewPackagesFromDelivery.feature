@Devstandalone 
Feature: The User Can't add new packages in delivery
    The user Create import ocean FCL shipment, , Add Delivery
    the user can't add new packages in delivery

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

    Scenario: Add Delivery
        Given the user open the shipment and navigate to RoutingsTab workspace
        And add a new Delivery leg with the following details
            | From        | Partner        |
            | FromPartner | Israeli Tenant |
            | To          | Partner        |
            | ToPartner   | Israeli Tenant |
        When  save the Delivery
        Then the direct should update successfully

    Scenario: The User can't add new packages in delivery
        Given the user in the packages tab
        When click add container
        Then the Blus button disappear in window

