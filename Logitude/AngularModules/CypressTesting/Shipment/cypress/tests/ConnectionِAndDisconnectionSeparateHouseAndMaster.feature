Feature: Connection And Disconnection Separate House And Master

  Scenario: Login And Open Shipments Workspace
    Given a user logged in
    And navigate to shipments workspace

  Scenario: Create Master Export Air Shipment
    Given a master Shipment with following details
      | ShipmentLevel | Direction | TransportMode | Agent            | MainCarriageFromPort | MainCarriageToPort |
      | Master        | E         | A             | IntegrationAgent | LHR                  | MIA                |
    When create shipment 
    Then the shipment should create successfully

 Scenario: Create House Export Air Shipment
    Given a house Shipment with following details
      | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
      | House         | E         | A             | Shipper1 | LHR                  | MIA                |
    When create shipment 
    Then the shipment should create successfully

  Scenario: Connect the house shipment to the master 
    Given the user in the master's Shipment tab
    When connect the house shipment 
    Then the shipment should connect successfully

  Scenario: Disconnect The House From The Master
    When the user disconnect the house shipment 
    Then the shipment should disconnect successfully  