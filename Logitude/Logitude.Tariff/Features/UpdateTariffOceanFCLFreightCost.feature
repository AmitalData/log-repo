@Pre-Prepare-Tariff-OceanFCL
Feature: Update Tariff Ocean FCL Freight Cost
	We want to update ocean FCL freight cost tariff.

Scenario: Update ocean FCL freight cost tariff
	Given an ocean FCL freight cost tariff
	And following ocean FCL freight cost tariff properties
		| property | Value                 |
		| Name     | updated specflow name |
		| Notes    | updated specflow note |
	When update ocean FCL freight cost tariff
	Then the ocean FCL freight cost tariff should update successfully