Feature:   Accounting Close , Accounting Reopen For Master And House Shipment
 The user creates a master Export Air Shipment, creates a house Export Air Shipmen within the master, operationally closes the shipment, 
 closes accounting, reopens accounting.
 
    Scenario: Create master export air shipment
        Given the user logged in and navigates to shipments workspace
        And a master Shipment with following details
            | ShipmentLevel        | Master    |
            | Direction            | Export    |
            | TransportMode        | Air       |
            | Agent                | TestAgent |
            | MainCarriageFromPort | LHR       |
            | MainCarriageToPort   | MIA       |
        When create master
        Then the master should create successfully 

     Scenario: Update routing tab
        Given the user in the master's rounting tab
        And edit main carriage leg with the following details
              | Airline      | AA     |
              | FlightNumber | Random |
              | MAWB         | Random |
              | ATD          | Today  |
        When update master
        Then the master should update successfully

    Scenario: Create house export air shipment inside the master
        Given the user in the master's Shipment tab
        When create house with "TestShipperExport" as Shipper
        Then the house should create successfully
        And the house should connect successfully

    Scenario: Close the master shipment Operationally 
        When close the master shipment operationally 
        Then the master should close operationally successfully
        And the house should close operationally successfully

  Scenario: Close master shipment accountly
    When close master Accountly
    Then the master should close successfully
    And the connected house should close successfully

  Scenario: Reopen master shipment accountly
    When reopen master Accountly
    Then the master should reopen successfully
    And the connected house should reopen successfully
