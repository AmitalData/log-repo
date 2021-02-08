Feature: Create Shipment Tests

  Scenario: Create Direct Export Air Shipment
    Given the user logged in and navigates to shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | Shipper1 | LHR                  | MIA                |
      | Direct        | Export    | Air           | Shipper1 | LHR                  | MIA                |
    When create shipment
    Then the shipment should create successfully







# | ShipmentLevel        | Direct   |
# | Direction            | Export   |
# | TransportMode        | Air      |
# | Shipper              | Shipper1 |
# | MainCarriageFromPort | LHR      |
# | MainCarriageToPort   | MIA      |