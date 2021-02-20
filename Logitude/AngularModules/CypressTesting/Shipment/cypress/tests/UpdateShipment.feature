@smoke @not-stable @all
Feature: Update direct export air shipment
  After the user logging in the system and navigate to shipments workspace
  will create a directe shipment, after that update general, orders
  partners, packages, receivables, routing and payables tabs.

  Scenario: Create direct export air shipment
    Given the user logged in and navigates to shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper           | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | TestShipperExport | LHR                  | MIA                |
    When create shipment
    Then the direct should create successfully

  Scenario: Update general tab
    Given the user fill "100" as GrossWeight and "MTA" as a MoveType
    When update shipment
    Then the direct should update successfully

  Scenario: Update orders tab
    Given the user add order package with the following details
      | Quantity | Length | Width | Height | GrossWeight |
      | 5        | 1      | 2     | 3      | 100         |
      | 5        | 1      | 2     | 3      | 100         |
    When update shipment
    Then the direct should update successfully

  Scenario: Update partners tab
    Given the user add partners with following details
      | Consignee           | Agent     | CustomsAgentExport | CustomsAgentImport | Notify1   | Notify2   | ShipperNotExporter | ConsigneeNotImporter | FreightForwarder | Coloader  | CustomClearancePoint | Consolidator | ReleasingAgent |
      | TestConsigneeExport | TestAgent | TestCustomAgent    | TestCustomAgent    | TestAgent | TestAgent | TestShipperExport  | TestConsigneeExport  | TestAgent        | TestAgent | TestWarehouse        | TestAgent    | TestAgent      |
    When update shipment
    Then the direct should update successfully

  Scenario: Update packages tab
    Given the user add package with the following details
      | Quantity | Length | Width | Height | GrossWeight |
      | 5        | 1      | 2     | 3      | 100         |
    When update shipment
    Then the direct should update successfully

  Scenario: Update receivables tab
    Given  the user fill receivables with the following details
      | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
      | AFT         | GRWT | 5        | 20        | EUR      | 4            |
    When update shipment
    Then the direct should update successfully

  Scenario: Update routing tab
    Given the user add new pickup
    And add delivery with "IntegrationAgent" as a partner routing
    And add pre carriage and on carriage from port "JFK" to port "MIA"
    When update shipment
    Then the direct should update successfully

  Scenario: Update payables tab
    Given the user add payable with the following details
      | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
      | AFT         | GRWT | 5        | 10        | EUR      | 4            |
    When update shipment
    Then the direct should update successfully