@smoke @stable @all
Feature: Shipment Update
  The user creates a Direct Export Air shipment and updates it.

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
    Then the direct should create successfully

  Scenario: Update general tab
    Given the user fill "100" as ValueOfGoods and "MTA" as a MoveType
    When update shipment
    Then the direct should update successfully

  Scenario: Update orders tab
    Given the user add order package with the following details
      | Quantity | Length | Width | Height | GrossWeight |
      | 5        | 1      | 2     | 3      | 100         |
    When update shipment
    Then the direct should update successfully

  Scenario: Delete order package
    Given the user delete the first order package
    When update shipment
    Then the direct should update successfully

  Scenario: Update packages tab
    Given the user add package with the following details
      | Quantity | Length | Width | Height | GrossWeight |
      | 5        | 1      | 2     | 3      | 100         |
    When update shipment
    Then the direct should update successfully

  Scenario: Delete packages tab
    Given the user delete the package in the packages tabs
    When update shipment
    Then the direct should update successfully

  Scenario: Update receivables tab
    Given  the user fill receivables with the following details
      | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
    When update shipment
    Then the direct should update successfully

  Scenario: Update routing tab
    Given add transshipments with "JFK" as via port and "AA" as airline
    And the user add new pickup
    And add delivery with "TestAgent" as a partner routing
    And add pre carriage and on carriage from port "JFK" to port "MIA"
    When update shipment
    Then the direct should update successfully

  Scenario: Update payables tab
    Given the user add payable with the following details
      | ChargesType  | AFT  |
      | UOM          | GRWT |
      | Quantity     | 5    |
      | UnitPrice    | 10   |
      | Currency     | EUR  |
      | ExchangeRate | 4    |
    When update shipment
    Then the direct should update successfully