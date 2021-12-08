@release
Feature: Operation Accounting Close and Reopen Direct Export Air Shipment
  The user creates a Direct Export Air Shipment, operationally closes the shipment,
  closes accounting, reopens accounting and operationally reopens the shipment.

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

  Scenario: Assert cannot add orders
    When navigates to orders tab
    Then the orders workspace should be dim

  Scenario: Assert cannot add packages
    When navigates to packages tab
    Then the packages workspace should be dim

  Scenario: Assert cannot add routing
    When navigates to routing tab
    Then the routing workspace should be dim
    And all fields of the main carriage leg should be dim

  Scenario: Assert cannot add or edit partners
    When navigates to partners tab
    Then the user should not be able to add or edit partners

  Scenario: Close direct shipment accountly
    When close shipment Accountly
    Then the shipment should close successfully

  Scenario: Assert cannot add payable
    When navigates to payables tab
    Then the user should not be able to add payable

  Scenario: Assert cannot add receivable
    When navigates to receivables tab
    Then the user should not be able to add receivable

  Scenario: Reopen direct shipment accountly
    When reopen shipment Accountly
    Then the shipment should reopen successfully

  Scenario: Assert add payable
    When navigates to payables tab
    Then the user should be able to add payable

  Scenario: Assert add receivable
    When navigates to receivables tab
    Then the user should be able to add receivable

  Scenario: Reopen direct shipment operationally
    When reopen shipment operationally
    Then the shipment should reopen successfully

  Scenario: Assert add orders
    When navigates to orders tab
    Then the user should be able to add orders

  Scenario: Assert add packages
    When navigates to packages tab
    Then the user should be able to add package

  Scenario: Assert add routing
    When navigates to routing tab
    Then the routing workspace should not be dim

  Scenario: Assert add or edit partners
    When navigates to partners tab
    Then the user should be able to add or edit partners