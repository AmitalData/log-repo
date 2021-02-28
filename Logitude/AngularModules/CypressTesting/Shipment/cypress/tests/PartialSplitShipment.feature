@smoke @not-stable @all
Feature: Partial Split shipment
    After the user logging in the system and navigate to shipments workspace
    will create a direct shipment , Add Packages then split it.

    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | TestShipperExport | LHR                  | MIA                |
        When create shipment
        Then the direct should create successfully

    Scenario: Add Packages
        Given the user open the shipment and navigate to packages workspace
        And a package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 12       | 100    | 100   | 100    | 1200        |
        When save shipment
        Then the direct shipment should save successfully

    Scenario: Partial Split Shipment
        When partial split the shipment with the following details
            | Quantity | Volume | GrossWeight |
            | 9        | 5.000  | 900.000     |
        Then the direct shipment should split into two shipment with packages with the following details
            | Quantity | Volume | GrossWeight |
            | 9        | 5.000  | 900.000     |
            | 3        | 7.000  | 300.000     |
