Feature: Create Shipment Tests

  Scenario: Login And Open Shipments Workspace
    Given User logged in
    And Go to shipments workspace

  Scenario Outline: Create Shipments
    Given A shipment details
      | LevelCode   | DirectionCode   | TransportModeCode   | ShipmentTypeCode   |
      | <LevelCode> | <DirectionCode> | <TransportModeCode> | <ShipmentTypeCode> |
    When Click create shipment button
    Then The create operation complete successfully
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
      | House     | E             | A                 |                  |
      | House     | I             | A                 |                  |
      | House     | D             | A                 |                  |
      | House     | R             | A                 |                  |
      | House     | E             | O                 | FCLD             |
      | House     | E             | O                 | LCLD             |
      | House     | I             | O                 | FCLD             |
      | House     | I             | O                 | LCLD             |
      | House     | D             | O                 | FCLD             |
      | House     | D             | O                 | LCLD             |
      | House     | R             | O                 | FCLD             |
      | House     | R             | O                 | LCLD             |
      | House     | E             | I                 | FTL              |
      | House     | E             | I                 | LTL              |
      | House     | I             | I                 | FTL              |
      | House     | I             | I                 | LTL              |
      | House     | R             | I                 | FTL              |
      | House     | R             | I                 | LTL              |