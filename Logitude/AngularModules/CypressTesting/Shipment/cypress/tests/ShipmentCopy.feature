@smoke @stable
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
        When create shipment
        Then the direct should create successfully

    Scenario: Copy Direct export air shipment
        Given the user open the direct shipment
        When copy the shipment
        Then a shipment copy should create successfully
