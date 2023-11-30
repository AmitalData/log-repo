@Pre-Prepare-Tariff-OceanLCL-Surcharge
Feature: Update Tariff Ocean LCL Surcharge Cost
	We want to update ocean LCL surcharge cost tariff.

Scenario: Update ocean LCL surcharge cost tariff
	Given an ocean LCL surcharge cost tariff
	And following ocean LCL surcharge cost tariff properties
		| property | Value                 |
		| Name     | updated specflow name |
		| Notes    | updated specflow note |
	When update ocean LCL surcharge cost tariff
	Then the ocean LCL surcharge cost tariff should update successfully