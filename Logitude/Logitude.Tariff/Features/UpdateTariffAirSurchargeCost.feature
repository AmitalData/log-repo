@Pre-Prepare-Tariff-Air-Surcharge
Feature: Update Tariff Air Surcharge Cost
	We want to update air surcharge cost tariff.

Scenario: Update air surcharge cost tariff
	Given an air surcharge cost tariff
	And following air surcharge cost tariff properties
		| property | Value                 |
		| Name     | updated specflow name |
		| Notes    | updated specflow note |
	When update air surcharge cost tariff
	Then the air surcharge cost tariff should update successfully