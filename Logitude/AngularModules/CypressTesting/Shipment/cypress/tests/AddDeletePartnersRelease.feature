@release
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

  Scenario: Add new agent from Edit agent screen
    Given navigates new agent wizerd
    And an agent with the following details
      | CompanyName | Testing agent Scenario |
      | LocalName   | Testing agent Scenario |
      | Address1    | 15 agent Street        |
      | Zip         | 0000                   |
      | City        | Anchorage              |
      | Country     | United States          |
      | State       | Alaska                 |
      | Phone       | 9999999999             |
      | Fax         | 999999                 |
    And an agent contact with the following details
      | AddContact    | Yes         |
      | EnglishName   | TestContact |
      | Position      | Developer   |
      | BusinessPhone | 2693000     |
      | Mobile        | 0599000000  |
      | Fax           | 888888      |
    When create agent
    Then the agent should create successfully

  Scenario: Select contact
    Given "TestContact" as a contact
    When update shipment
    Then the direct should update successfully

  Scenario: Set agent my customer
    Given set agent as my customer
    When update shipment
    Then the direct should update successfully