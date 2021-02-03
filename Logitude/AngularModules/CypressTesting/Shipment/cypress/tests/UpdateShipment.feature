Feature: Edit Direct Export Air Shipment

  Scenario: Create Direct Export Air Shipment
    Given the user logged in and navigates to shipments workspace
    And a direct shipment with the following details
      | ShipmentLevel | Direction | TransportMode | Shipper  | MainCarriageFromPort | MainCarriageToPort |
      | Direct        | Export    | Air           | Shipper1 | LHR                  | MIA                |
    When create shipment
    Then the direct should create successfully
    
  Scenario: Edit Shipment by filling general tab
    Given the user in the general tab
    And  fill random GrossWeight and "MTA" as a MoveType 
    When save shipment
    Then the save operation complete successfully

  Scenario: Edit Shipment by filling Orders tab
    Given the user in the Orders tab
    And  fill Orders tab with random number of Packages
    When save shipment
    Then the save operation complete successfully

  Scenario: Edit Shipment by filling Partners tab
    Given the user in the Partners tab
    And  fill Partners tab with following details
     | Agent            | CustomsAgentImport     | CustomsAgentExport     |
     | IntegrationAgent | InegrationCustomsAgent | InegrationCustomsAgent |
    When save shipment
    Then the save operation complete successfully

  Scenario: Edit Shipment by filling Packages tab
    Given the user in the Packages tab
    And  fill Packages tab with random number of Packages
    When save shipment
    Then the save operation complete successfully

  Scenario: Edit Shipment by filling Receivables tab
    Given the user in the Receivables tab
    And  fill Receivables with the following details
     | ChargesType | UOM  | Quantity | UnitPrice | Currency | ExchangeRate |
     | AFT         | GRWT | 5        | 20        | EUR      | 4            |
    When save shipment
    Then the save operation complete successfully

  Scenario: Add Pickup Routing
    Given the user in the Routings tab
    And  add new pickup routing 
    When save shipment window
    Then the save operation complete successfully

  Scenario: Add Pre Carriage And On Carriage Routings
    Given the user in the Routings tab
    And  add carriage routings from port "JFK" to port "MIA" 
    When save shipment 
    Then the save operation complete successfully

  Scenario: Add Delivery Routing
    Given the user in the Routings tab
    And  add new delivery with "IntegrationAgent" as a partner routing
    When save shipment window
    Then the save operation complete successfully

  Scenario: Fill Payables Tab
    Given the user in the Payables tab
    And  add Payables with "AFT" as a ChargesType, "EUR" as a Currency and "GRWT" as UOM 
    When save shipment
    Then the save operation complete successfully