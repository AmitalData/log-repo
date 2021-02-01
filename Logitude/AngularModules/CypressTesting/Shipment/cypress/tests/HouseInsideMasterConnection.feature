Feature: House Creation And Disconnecting Inside The Master

  Scenario: Create Master Export Air Shipment
    Given the user logged in and navigates to shipments workspace
    And a master Shipment with following details
      | ShipmentLevel | Direction | TransportMode | Agent            | MainCarriageFromPort | MainCarriageToPort |
      | Master        | Export    | Air           | IntegrationAgent | LHR                  | MIA                |
    When create shipment
    Then the master should create successfully

  Scenario: Create House Export Air Shipment Inside The Master
    Given the user in the master's Shipment tab
    When create house with "Shipper1" as Shipper
    Then the house should create and connect successfully 

  Scenario: Disconnect The House From The Master
    When disconnect shipment
    Then the shipment should disconnect successfully  