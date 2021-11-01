@devsmoke @release @stable @all
Feature: UOM
    The user update system defaults, creates a direct export air shipment, change shipment ratio,
    update packages tab, add payable, change gross weight unit code.

    Scenario: Update System Defaults
        Given the user logged in and navigates to "System Defaults" in maintenance menu
        And "CBM" as volume unit
        And "Kilogram" as Gross and Chargeable Weight Unit
        When update system defaults
        Then the system defaults should update successfully

    Scenario: Create direct export air shipment
        Given the user navigates to shipments workspace
        And a direct shipment with the following details
            | ShipmentLevel        | Direct      |
            | Direction            | Export      |
            | TransportMode        | Air         |
            | Shipper              | TestCompany |
            | MainCarriageFromPort | LHR         |
            | MainCarriageToPort   | MIA         |
        When create shipment
        Then the direct should create successfully

    Scenario: Change shipment ratio
        Given open the shipment and navigates packages tab
        Given the user change the shipment ratio to "5"
        When update shipment
        Then the direct should update successfully

    Scenario: Update packages tab
        Given the user add a package with the following details
            | Quantity | Length | Width | Height | GrossWeight |
            | 1        | 100    | 100   | 100    | 500         |
        When update shipment
        Then the direct should update successfully

    Scenario: Add Payable
        Given the user navigates to payable wizerd
        And a payable with "AFT" as a charges type
        Then the Quantity should should has "500" as a value

    Scenario: Change Gross Weight Unit Code
        Given the user navigates to packages tab
        And a Gross Weight with "Pound" as a value
        When update shipment
        Then the direct should update successfully

    Scenario: Add Payable
        Given the user navigates to payable wizerd
        And a payable with "AFT" as a charges type
        Then the Quantity should should has "227" as a value