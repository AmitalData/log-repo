Feature: Cancel Shipment 

  Scenario: Login And Open Shipments Workspace
    Given the user logged in
    And navigate to shipments workspace

  Scenario: Create Direct Export Air Shipment
    Given a direct shipment with the following details 
      | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | E         | A             | Shipper1 | LHR                  | MIA                |
    When create shipment 
    Then the shipment should create successfully

    Scenario: Cancel Direct Shipment
    Given the user open the direct shipment 
    When cancel the shipment 
    Then the shipment should cancel successfully 