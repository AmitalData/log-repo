Feature: Update Shipments

  Scenario: Login And Open Shipments Workspace
    Given User logged in
    And Go to shipments workspace
    And Open the direct Shipment "CreatedShipmentsData/DirectEA.json"

  Scenario: Edit Direct Shipment by filling general tab
    Given The user in the general work space
    When Fill general tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Direct Shipment by filling Orders tab
    Given The user in the Orders work space
    When Fill Orders tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Direct Shipment by filling Partners tab
    Given The user in the Partners work space
    When Fill Partners tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Direct Shipment by filling Packages tab
    Given The user in the Packages work space
    When Fill Packages tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Direct Shipment by filling Receivables tab
    Given The user in the Receivables work space
    When Fill Receivables tab and click save button
    Then The save operation complete successfully