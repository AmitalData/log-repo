Feature: Cancel shipment
  After the user logging in the system and navigate to shipments workspace
  will create a direct shipment and cancel it.
  
  Scenario: Create direct export air shipment
    Given the user logged in and navigates to shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | TestShipperExport | LHR                  | MIA                |
    When create shipment
    Then the direct should create successfully

  Scenario: Cancel direct shipment
    Given the user open the shipment
    When cancel the shipment
    Then the shipment should cancel successfully