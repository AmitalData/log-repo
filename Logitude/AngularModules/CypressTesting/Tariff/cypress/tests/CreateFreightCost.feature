@release
Feature: Create New Freight Cost Tariff
    The authenticated user will create new air, ocean LCL, and ocean FCL freight cost tariff.

    Scenario: Login and create new air freight cost
        Given the user logged in and navigate to tariff workspace
        And an air freight cost with the following details
            | Name               | Seller | StartDate | Product |
            | TestAirFreightCost | AA     | Today     | General |
        And the follwing All-In charges
            | Name             |
            | Agent Commission |
            | Air Waybill Fee  |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Create new ocean LCL freight cost
        Given an ocean LCL freight cost with the following details
            | Name                    | Seller | StartDate |
            | TestOceanLCLFreightCost | MAEU   | Today     |
        And the follwing All-In charges
            | Name                     |
            | Bunker Adjustment Factor |
            | B/L Fee                  |
        When create freight cost
        Then the freight cost should create successfully

    Scenario: Create new ocean FCL freight cost
        Given an ocean FCL freight cost with the following details
            | Name                    | Seller | StartDate |
            | TestOceanFCLFreightCost | MAEU   | Today     |
        And the follwing All-In charges
            | Name                     |
            | Bunker Adjustment Factor |
            | B/L Fee                  |
        When create freight cost
        Then the freight cost should create successfully