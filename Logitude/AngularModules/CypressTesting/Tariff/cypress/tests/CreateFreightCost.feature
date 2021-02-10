@release
Feature: Create New Freight Cost Tariff
  The authenticated user will create new air, ocean LCL, and ocean FCL freight cost tariff.

  Scenario: Create New Air Freight Cost
    Given the user logged in and navigate to tariff workspace
    And an air freight cost with the following details
      | Name               | Seller | StartDate | Product |
      | TestAirFreightCost | AA     | Today     | General |
    And add the follwing All-In charges
      | Name             |
      | Agent Commission |
      | Air Waybill Fee  |
    When create freight cost
    Then the freight cost should create successfully


  Scenario: Create New Ocean LCL Freight Cost
    Given an ocean LCL freight cost with the following details
      | Name                    | Seller | StartDate |
      | TestOceanLCLFreightCost | MAEU   | Today     |
    And add the follwing All-In charges
      | Name                     |
      | Bunker Adjustment Factor |
      | B/L Fee                  |
    When create freight cost
    Then the freight cost should create successfully


  Scenario: Create New Ocean FCL Freight Cost
    Given an ocean FCL freight cost with the following details
      | Name                    | Seller | StartDate |
      | TestOceanFCLFreightCost | MAEU   | Today     |
    And add the follwing All-In charges
      | Name                     |
      | Bunker Adjustment Factor |
      | B/L Fee                  |
    When create freight cost
    Then the freight cost should create successfully
