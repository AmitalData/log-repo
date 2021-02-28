@smoke @stable @all @shipments2
Feature: Copy direct export air shipment
    After the user logging in the system and navigate to shipments workspace
    will will create a direct shipment and copy it.
    
    Scenario: Create direct export air shipment
        Given the user logged in and navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | TestShipperExport | LHR                  | MIA                |
        When create shipment
        Then the direct should create successfully

    Scenario: Copy Direct export air shipment
        Given the user open the direct shipment
        When copy the shipment
        Then a shipment copy should create successfully
