Feature:Copy Direct Export Air Shipment

    Scenario: Login And Open Shipments Workspace
        Given the user logged in
        And navigates to shipments workspace

    Scenario: Create Direct Export Air Shipment
        Given a direct shipment with the following details
            | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
            | Direct        | Export    | Air           | Shipper1 | LHR                  | MIA                |
        When create shipment 
        Then the direct should create successfully

    Scenario: Copy Direct Export Air Shipment
        Given the user open the direct shipment
        When copy the shipment
        Then a shipment copy should create successfully
