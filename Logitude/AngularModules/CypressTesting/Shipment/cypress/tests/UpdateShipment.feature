Feature: Edit Direct Export Air Shipment

  Scenario: Login And Open Shipments Workspace
    Given User logged in
    And Go to shipments workspace
    And Create a new Direct shipment

# Using the created shipment in the first scenario do the following.
  Scenario: Edit Shipment by filling general tab
    Given The user in the general tab
    When Fill general tab with a random GrossWeight and "MTA" as a MoveType and click save button
    Then The save operation complete successfully

  Scenario: Edit Shipment by filling Orders tab
    Given The user in the Orders tab
    When Fill Orders tab with random number of Packages and click save button
    Then The save operation complete successfully

  Scenario: Edit Shipment by filling Partners tab
    Given The user in the Partners tab
    When Fill Partners tab with following partners and click save button
     | Agent            | CustomsAgentImport     | CustomsAgentExport     |
     | IntegrationAgent | InegrationCustomsAgent | InegrationCustomsAgent |
    Then The save operation complete successfully

  Scenario: Edit Shipment by filling Packages tab
    Given The user in the Packages tab
    When Fill Packages tab with random number of Packages and click save button
    Then The save operation complete successfully

  Scenario: Edit Shipment by filling Receivables tab
    Given The user in the Receivables tab
    When Fill Receivables tab  with a random UnitPrice and "AFT" as a ChargesType and click save button
    Then The save operation complete successfully

  Scenario: Add Pickup Routing
    Given The user in the Routings tab
    When Add new pickup routing and click save button
    Then The save operation complete successfully

  Scenario: Add Pre Carriage And On Carriage Routings
    Given The user in the Routings tab
    When Add carriage routings from port "JFK" to port "MIA" and click save button
    Then The save operation complete successfully

  Scenario: Add Delivery Routing
    Given The user in the Routings tab
    When Add new delivery with "IntegrationAgent" as a partner routing and click save button
    Then The save operation complete successfully

  Scenario: Fill Payables Tab
    Given The user in the Payables tab
    When Add Payables with "AFT" as a ChargesType, "EUR" as a Currency and random UOM and click save button
    Then The save operation complete successfully
    