@Pre-Prepare-Tariff-Air
Feature: Update Tariff Air Freight Cost
	We want to update air freight cost tariff.

Scenario: Update air freight cost tariff
	Given an air freight cost tariff
	And following air freight cost tariff properties
		| property | Value                 |
		| Name     | updated specflow name |
		| Notes    | updated specflow note |
	When update air freight cost tariff
	Then the air freight cost tariff should update successfully