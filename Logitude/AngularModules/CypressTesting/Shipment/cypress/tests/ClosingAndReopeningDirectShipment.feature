Feature:  Closing And Reopening Direct Shipment 

  Scenario: Create Direct Export Air Shipment
    Given the user logged in and navigates to shipments workspace 
    And a direct shipment with the following details 
      | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | Shipper1 | LHR                  | MIA                |
    When create shipment 
    Then the shipment should create successfully

  Scenario: Close Direct Shipment operationally
    Given the user in the direct's shipment rounting tab
    And  edit Main Carriage Leg adding the Airline "AA"   
    When close shipment operationally
    Then the shipment should close successfully 

  Scenario: Close Direct Shipment Accountly  
    When close shipment Accountly
    Then the shipment should close successfully 

  Scenario: Reopen Direct Shipment Accountly
    When reopen shipment Accountly
    Then The shipment should Reopen successfully

  Scenario: Reopen Direct Shipment operationally
    When reopen shipment operationally
    Then The shipment should Reopen successfully 