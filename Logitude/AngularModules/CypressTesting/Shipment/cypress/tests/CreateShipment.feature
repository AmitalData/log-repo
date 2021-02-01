Feature: Create Shipment Tests

  Scenario: Login And Open Shipments Workspace
    Given the user logged in
    And navigate to shipments workspace

  Scenario: Create Direct Export Air Shipment
    Given a direct shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | Shipper1 | LHR                  | MIA                |
    When create shipment
    Then the direct should create successfully