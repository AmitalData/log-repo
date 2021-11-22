@release
Feature: UOMRelease
    The user update system defaults, creates a direct export air shipment, change shipment ratio,
    update packages tab, add payable, add different unit of measurments.

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
            | 1        | 100    | 200   | 100    | 500         |
        When update shipment
        Then the direct should update successfully
    # fixed
    Scenario: Add Payable with fixed as  charge type
        Given the user navigates to payable wizard
        And a payable with "AFT" as a charges type
        And a "FIXD" as a UOM
        Then the quantity should has "1" as a value

    # Gross weight
    Scenario: Add Payable with gross weight as  charge type
        Given the user navigates to payable wizard
        And a payable with "AFT" as a charges type
        And a "GRWT" as a UOM
        Then the quantity should has "500" as a value

    Scenario: Add Payable with gross weight in metric ton as  charge type
        Given the user navigates to payable wizard
        And a payable with "AFT" as a charges type
        And a "GWTN" as a UOM
        Then the quantity should has "0.5" as a value

    Scenario: Change Gross Weight Unit Code
        Given the user navigates to packages tab
        And a Gross Weight with "Pound" as a value
        When update shipment
        Then the direct should update successfully

    Scenario: Add Payable with gross weight in kilogram as  charge type
        Given the user navigates to payable wizard
        And a payable with "AFT" as a charges type
        And a "GWKG" as a UOM
        Then the quantity should has "226.796" as a value

    #Quantity
    Scenario: Add Payable with quantity as charge type
        Given the user navigates to payable wizard
        And a payable with "AFT" as a charges type
        And a "QTY" as a UOM
        Then the quantity should has "1" as a value
    # Volume
    Scenario: Add Payable with volume as charge type
        Given the user navigates to payable wizard
        And a payable with "AFT" as a charges type
        And a "VOLU" as a UOM
        Then the quantity should has "2" as a value


          # Chargable weight
    Scenario: Add Payable with chargable weight as  charge type
        Given the user navigates to payable wizard
        And a payable with "AFT" as a charges type
        And a "CHWT" as a UOM
        Then the quantity should has "400" as a value