@smoke @stable @all @shipments2 @stable
Feature:  Closing and reopening direct shipment
  After the user logging in the system and navigate to shipments workspace
  will create a direct shipment, after that cclose and reopen it operationally and Accountly.

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

  Scenario: Close direct shipment operationally
    Given the user in the direct's shipment rounting tab
    And edit main carriage leg with the following details
      | Airline      | AA     |
      | FlightNumber | Random |
      | MAWB         | Random |
      | ATD          | Today  |
    When close shipment operationally
    Then the shipment should close successfully

  Scenario: Close direct shipment accountly
    When close shipment Accountly
    Then the shipment should close successfully

  Scenario: Reopen direct shipment accountly
    When reopen shipment Accountly
    Then the shipment should reopen successfully

  Scenario: Reopen direct shipment operationally
    When reopen shipment operationally
    Then the shipment should reopen successfully