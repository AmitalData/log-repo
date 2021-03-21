@smoke @stable
Feature: House connection and disconnection inside master
  After the user logging in the system and navigate to shipments workspace
  will create a master shipment, after that create house inside master and disconnect it.

  Scenario: Create master export air shipment
    Given the user logged in and navigates to shipments workspace
    And a master Shipment with following details
      | ShipmentLevel        | Master    |
      | Direction            | Export    |
      | TransportMode        | Air       |
      | Agent                | TestAgent |
      | MainCarriageFromPort | LHR       |
      | MainCarriageToPort   | MIA       |
    When create shipment
    Then the master should create successfully

  Scenario: Create house export air shipment inside the master
    Given the user in the master's Shipment tab
    When create house with "TestShipperExport" as Shipper
    Then the house should create and connect successfully

  Scenario: Disconnect the house shipment
    When disconnect shipment
    Then the shipment should disconnect successfully