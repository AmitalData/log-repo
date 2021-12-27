@smoke2 @stable
Feature: Shipment Prorate
    The user creates a Master Export Air shipment, creates a House Export Air shipment from withing the Master shipment,
    add package, create another house, add package, generate packages in master from houses, add payable in master
    and make some assertion to make sure prorate works fine

    Scenario: Create master export air shipment
        Given the user logged in and navigates to shipments workspace
        And a master Shipment with following details
            | ShipmentLevel        | Master    |
            | Direction            | Export    |
            | TransportMode        | Air       |
            | Agent                | TestAgent |
            | MainCarriageFromPort | LHR       |
            | MainCarriageToPort   | MIA       |
        When create shipment
        Then the master should create successfully

    Scenario: Create the first house export air shipment inside the master
        Given the user in the master's Shipment tab
        When create house with "TestShipperExport" as Shipper
        Then the house should create successfully
        And the house should connect successfully

    Scenario: Add Packages to the first house shipment
        Given the user navigate to the first house workspace
        And fill the following package details
            | Quantity | Length | Width | Height | GrossWeight |
            | 1        | 1      | 1     | 1      | 12          |
        When save shipment
        Then the shipment should save successfully

    Scenario: Create the seconed house export air shipment inside the master
        When create house with "TestShipperExport" as Shipper
        Then the house should create successfully
        And the house should connect successfully

    Scenario: Add Packages to the seconed house shipment
        Given the user navigate to the seconed house workspace
        And fill the following package details
            | Quantity | Length | Width | Height | GrossWeight |
            | 1        | 1      | 1     | 1      | 24          |
        When save shipment
        Then the shipment should save successfully

    Scenario: generate the house shipment Packages in the master shipment
        Given the user navigate to packages workspace in master shipment
        And bress on build from shipments
        When save the shipment
        Then the shipment should save successfully

    Scenario: Add Payables
        Given a payable with the following details
            | ChargesType  | AFT  |
            | UOM          | GRWT |
            | Quantity     | 36   |
            | UnitPrice    | 2    |
            | Currency     | EUR  |
            | ExchangeRate | 4    |
        When add payables
        Then the payables should add successfully

    Scenario: Assert prorate divided the values in the correct way
        When press on the plus icon to see the prorate details
        Then the prorate should be divided the values in the correct way

    Scenario: Assert house has a payable from the master
        Given the user in the first house payable tab
        When press on edit payable button
        Then this message "This payable is connected to master" should be printed