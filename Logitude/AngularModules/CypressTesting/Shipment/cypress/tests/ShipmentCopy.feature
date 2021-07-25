@smoke @dev @all
Feature: Copy Direct Export Air Shipment
    The user creates a Direct Export Air shipment and copies it.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct            |
            | Direction            | Export            |
            | TransportMode        | Air               |
            | Shipper              | TestShipperExport |
            | MainCarriageFromPort | LHR               |
            | MainCarriageToPort   | MIA               |
            | Consignee            | aaa               |
            | DescriptionOfGoods   | toys              |
        When create shipment
        Then the direct should create successfully

    Scenario: Add Packages
        Given the user open the shipment and navigate to packages workspace
        And a package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 12       | 100    | 100   | 100    | 1200        |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Copy Direct export air shipment
        When copy the shipment
        Then a shipment copy should create successfully
