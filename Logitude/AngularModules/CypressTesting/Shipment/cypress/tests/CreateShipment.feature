Feature: Create Shipment Tests

  Scenario: Login And Open Shipments Workspace
    Given User logged in
    And Go to shipments workspace

  Scenario: Create Direct Export Air Shipment
    Given Shipment details
      | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | E         | A             | Shipper1 | LHR                  | MIA                |
    When Click create shipment button
    Then The create operation completed successfully