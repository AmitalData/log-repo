@release
Feature: Create New Surcharge Cost Tariff
    The authenticated user will create new air, ocean LCL, and ocean FCL surcharge cost tariff.

    Scenario: Create New Air Surcharge Cost
        Given the user logged in and navigate to tariff workspace
        And an air surcharge cost with the following details
            | Name                 | Seller |
            | TestAirSurchargeCost | AA     |
        And add the follwing surcharges
            | Name             |
            | Agent Commission |
            | Air Waybill Fee  |
        When create surcharge cost
        Then the surcharge cost should create successfully

    Scenario: Create New Ocean LCL Surcharge Cost
        Given an ocean LCL surcharge cost with the following details
            | Name                      | Seller |
            | TestOceanLCLSurchargeCost | MAEU   |
        And add the follwing surcharges
            | Name                     |
            | Bunker Adjustment Factor |
            | B/L Fee                  |
        When create surcharge cost
        Then the surcharge cost should create successfully

    Scenario: Create New Ocean FCL Surcharge Cost
        Given an ocean FCL surcharge cost with the following details
            | Name                      | Seller |
            | TestOceanFCLSurchargeCost | MAEU   |
        And add the follwing surcharges
            | Name                     |
            | Bunker Adjustment Factor |
            | B/L Fee                  |
        When create surcharge cost
        Then the surcharge cost should create successfully