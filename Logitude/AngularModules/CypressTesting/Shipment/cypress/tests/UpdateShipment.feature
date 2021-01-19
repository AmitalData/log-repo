Feature: Edit Direct Export Air Shipment

  Scenario: Login And Open Shipments Workspace
    Given User logged in
    And Go to shipments workspace
    And Open shipment "CreatedShipmentsData/DirectEA.json"

  Scenario: Edit Shipment by filling general tab
    Given The user in the general tab
    When Fill general tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Shipment by filling Orders tab
    Given The user in the Orders tab
    When Fill Orders tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Shipment by filling Partners tab
    Given The user in the Partners tab
    When Fill Partners tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Shipment by filling Packages tab
    Given The user in the Packages tab
    When Fill Packages tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Shipment by filling Receivables tab
    Given The user in the Receivables tab
    When Fill Receivables tab and click save button
    Then The save operation complete successfully

  Scenario: Add Pickup Routing
    Given The user in the Routings tab
    When Add new pickup routing and click save button
    Then The save operation complete successfully

  Scenario: Add Pre Carriage And On Carriage Routings
    Given The user in the Routings tab
    When Add carriage routings and click save button
    Then The save operation complete successfully

  Scenario: Add Delivery Routing
    Given The user in the Routings tab
    When Add new delivery routing and click save button
    Then The save operation complete successfully

  Scenario: Fill Payables Tab
    Given The user in the Payables tab
    When Add Payables and click save button
    Then The save operation complete successfully
    