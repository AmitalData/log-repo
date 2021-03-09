@smoke @all @shipments2 @stable
Feature: Connect and disconnect separate house and master
  After the user logging in the system and navigate to shipments workspace
  will create a Separate master and house shipments
  after that connect/disconnect them together.

  Scenario: Create master export air shipment
    Given the user logged in and navigates to shipments workspace
    And a master Shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Agent            | MainCarriageFromPort | MainCarriageToPort |
      | Master        | Export    | Air           | IntegrationAgent | LHR                  | MIA                |
    When create shipment
    Then the master should create successfully

  Scenario: Create house export air shipment
    Given a house Shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
      | House         | Export    | Air           | TestShipperExport | LHR                  | MIA                |
    When create shipment
    Then the house should create successfully

  Scenario: Connect the house shipment to the master
    Given the user in the master's Shipment tab
    When connect the house shipment
    Then the shipment should connect successfully

  Scenario: Disconnect the house shipment
    When the user disconnect the house shipment
    Then the shipment should disconnect successfully