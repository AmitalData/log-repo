Feature: Create Shipment Tests

  Scenario: Create Direct Export Air Shipment
    Given the user logged in and navigates to shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | TestShipperExport | LHR                  | MIA                |
    When create shipment
    Then the shipment should create successfully