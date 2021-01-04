Feature: Create Shipments Test

  Scenario: Login And Open Shipments Workspace
    Given User logged in successfully
    And Open shipments workspace


  Scenario Outline: Create Shipments
    Given "<LevelCode>" shipment with direction "<DirectionCode>" and transport mode "<TransportModeCode>" and type "<ShipmentTypeCode>"
    When Click create shipment
    Then The shipment should created successfully
    Examples:
      | LevelCode | DirectionCode | TransportModeCode | ShipmentTypeCode |
      | Direct    | E             | A                 |                  |
      | Direct    | I             | A                 |                  |
      | Direct    | D             | A                 |                  |
      | Direct    | R             | A                 |                  |
      | Direct    | E             | O                 | FCLD             |
      | Direct    | E             | O                 | LCLD             |
      | Direct    | I             | O                 | FCLD             |
      | Direct    | I             | O                 | LCLD             |
      | Direct    | D             | O                 | FCLD             |
      | Direct    | D             | O                 | LCLD             |
      | Direct    | R             | O                 | FCLD             |
      | Direct    | R             | O                 | LCLD             |
      | Direct    | E             | I                 | FTL              |
      | Direct    | E             | I                 | LTL              |
      | Direct    | I             | I                 | FTL              |
      | Direct    | I             | I                 | LTL              |
      | Direct    | D             | I                 | FTL              |
      | Direct    | D             | I                 | LTL              |
      | Direct    | R             | I                 | FTL              |
      | Direct    | R             | I                 | LTL              |