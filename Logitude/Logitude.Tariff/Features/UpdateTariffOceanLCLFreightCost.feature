@Pre-Prepare-Tariff-OceanLCL
Feature: Update Tariff Ocean LCL Freight Cost
	We want to update ocean LCL freight cost tariff.

Scenario: Update ocean LCL freight cost tariff
	Given an ocean LCL freight cost tariff
	And following ocean LCL freight cost tariff properties
		| property | Value                 |
		| Name     | updated specflow name |
		| Notes    | updated specflow note |
	When update ocean LCL freight cost tariff
	Then the ocean LCL freight cost tariff should update successfully