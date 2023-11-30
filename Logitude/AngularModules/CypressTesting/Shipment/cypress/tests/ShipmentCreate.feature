@release @stable
Feature: Create Shipments
  The user creates a direct export air shipment, direct import ocean shipment,
  direct domestic inland shipment and direct Drop Air shipment.

  Scenario: Create direct export air shipment
    Given the user logged in and navigates to shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel        | Direct            |
      | Direction            | Export            |
      | TransportMode        | Air               |
      | Shipper              | TestShipperExport |
      | MainCarriageFromPort | LHR               |
      | MainCarriageToPort   | MIA               |
    When create shipment
    Then the shipment should create successfully

  Scenario: Create direct import ocean shipment
    Given a direct shipment with the following details
      | ShipmentLevel        | Direct            |
      | Direction            | Import            |
      | TransportMode        | Ocean             |
      | ShipmentType         | FCL               |
      | Consignee            | TestShipperExport |
      | MainCarriageFromPort | LHR               |
      | MainCarriageToPort   | MIA               |
    When create shipment
    Then the shipment should create successfully

  Scenario: Create direct domestic inland shipment
    Given a direct shipment with the following details
      | ShipmentLevel        | Direct            |
      | Direction            | Domestic          |
      | TransportMode        | Inland            |
      | ShipmentType         | FTL               |
      | Consignee            | TestShipperExport |
      | Shipper              | TestShipperExport |
      | MainCarriageFromPort | LHR               |
      | MainCarriageToPort   | MIA               |
    When create shipment
    Then the shipment should create successfully

  Scenario: Create direct drop air shipment
    Given a direct shipment with the following details
      | ShipmentLevel        | Direct            |
      | Direction            | Drop              |
      | TransportMode        | Air               |
      | Shipper              | TestShipperExport |
      | MainCarriageFromPort | LHR               |
      | MainCarriageToPort   | MIA               |
    When create shipment
    Then the shipment should create successfully