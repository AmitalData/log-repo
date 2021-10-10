@smoke @stable @all
Feature: Add Delete partners
  The user creates a Direct Export Air shipment, add partners and delete partners it.

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

  Scenario: Update partners tab
    Given the user add partners with following details
      | Consignee            | TestConsigneeExport |
      | Agent                | TestAgent           |
      | CustomsAgentExport   | TestCustomAgent     |
      | CustomsAgentImport   | TestCustomAgent     |
      | Notify1              | TestAgent           |
      | Notify2              | TestAgent           |
      | ShipperNotExporter   | TestShipperExport   |
      | ConsigneeNotImporter | TestConsigneeExport |
      | FreightForwarder     | TestAgent           |
      | Coloader             | TestAgent           |
      | CustomClearancePoint | TestWarehouse       |
      | Consolidator         | TestAgent           |
      | ReleasingAgent       | TestAgent           |
    When update shipment
    Then the direct should update successfully

  Scenario: Delete partners tab
    Given the user delete the below partners
      | Consignee            | Consignee            |
      | Agent                | Agent                |
      | CustomsAgentExport   | CustomAgentExport    |
      | CustomsAgentImport   | CustomAgentImport    |
      | Notify1              | Notify1              |
      | Notify2              | Notify2              |
      | ShipperNotExporter   | ShipperNotExporter   |
      | ConsigneeNotImporter | ConsigneeNotImporter |
      | FreightForwarder     | FreightForwarder     |
      | Coloader             | Coloader             |
      | CustomClearancePoint | CustomClearancePoint |
      | Consolidator         | Consolidator         |
      | ReleasingAgent       | ReleasingAgent       |
    When update shipment
    Then the direct should update successfully