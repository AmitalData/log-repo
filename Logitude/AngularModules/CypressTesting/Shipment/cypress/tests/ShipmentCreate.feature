@Release @stable @all
Feature: Create Direct Export Air Shipment
  The user creates a Direct Export Air shipment.

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