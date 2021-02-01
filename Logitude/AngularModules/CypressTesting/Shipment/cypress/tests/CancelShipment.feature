Feature: Cancel Shipment 

  Scenario: Create Direct Export Air Shipment
    Given the user logged in and navigates to shipments workspace 
    And a direct shipment with the following details 
      | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | E         | A             | Shipper1 | LHR                  | MIA                |
    When create shipment 
    Then the direct should create successfully

  Scenario: Cancel Direct Shipment
    Given the user open the direct shipment
    When cancel the shipment
    Then the shipment should cancel successfully