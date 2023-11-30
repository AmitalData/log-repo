@Pre-Prepare-Tariff-OceanFCL-Surcharge
Feature: Update Tariff Ocean FCL Surcharge Cost
	We want to update ocean FCL surcharge cost tariff.

Scenario: Update ocean FCL surcharge cost tariff
	Given an ocean FCL surcharge cost tariff
	And following ocean FCL surcharge cost tariff properties
		| property | Value                 |
		| Name     | updated specflow name |
		| Notes    | updated specflow note |
	When update ocean FCL surcharge cost tariff
	Then the ocean FCL surcharge cost tariff should update successfully