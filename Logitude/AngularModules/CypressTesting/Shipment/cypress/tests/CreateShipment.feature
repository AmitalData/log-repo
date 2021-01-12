Feature: Create Shipment Tests

  Scenario: Login And Open Shipments Workspace
    Given User logged in
    And Go to shipments workspace

  Scenario Outline: Create Shipments
    Given Shipment details
      | LevelCode   | DirectionCode   | TransportModeCode   | ShipmentTypeCode   |
      | <LevelCode> | <DirectionCode> | <TransportModeCode> | <ShipmentTypeCode> |
    When Click create shipment button
    Then The create operation completed successfully
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
      | Master    | E             | A                 |                  |
      | Master    | I             | A                 |                  |
      | Master    | D             | A                 |                  |
      | Master    | R             | A                 |                  |
      | Master    | E             | O                 | FCLD             |
      | Master    | E             | O                 | LCLD             |
      | Master    | E             | O                 | Groupage         |
      | Master    | I             | O                 | FCLD             |
      | Master    | I             | O                 | LCLD             |
      | Master    | I             | O                 | Groupage         |
      | Master    | D             | O                 | FCLD             |
      | Master    | D             | O                 | LCLD             |
      | Master    | D             | O                 | Groupage         |
      | Master    | R             | O                 | FCLD             |
      | Master    | R             | O                 | LCLD             |
      | Master    | R             | O                 | Groupage         |
      | Master    | E             | I                 | FTL              |
      | Master    | E             | I                 | LTL              |
      | Master    | E             | I                 | Groupage         |
      | Master    | I             | I                 | FTL              |
      | Master    | I             | I                 | LTL              |
      | Master    | I             | I                 | Groupage         |
      | Master    | R             | I                 | FTL              |
      | Master    | R             | I                 | LTL              |
      | Master    | R             | I                 | Groupage         |