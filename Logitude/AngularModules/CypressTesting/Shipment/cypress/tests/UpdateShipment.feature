Feature: Update Shipments

  Scenario: Login And Open Shipments Workspace
    Given User logged in successfully
    And Go to shipments workspace
    And Open the direct Shipment "ResponseData/DEAShipment.json"

  Scenario: Edit Direct Shipment by filling general tab
    Given The user in the general work space
    When The user fill general tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Direct Shipment by filling Orders tab
    Given The user in the Orders work space
    When The user fill Orders tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Direct Shipment by filling Partners tab
    Given The user in the Partners work space
    When The user fill Partners tab and click save button
    Then The save operation complete successfully

  Scenario: Edit Direct Shipment by filling Packages tab
    Given The user in the Packages work space
    When The user fill Packages tab and click save button
    Then The save operation complete successfully