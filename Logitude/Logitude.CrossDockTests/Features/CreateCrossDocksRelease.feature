@Pre-Prepare-release
Feature: Create Cross Docks Release
    With pre-prepared entry data
	We want to create release cross docks.

Scenario: Create release cross dock
	Given a release cross dock with the following properties
		| property                 | Value  |
		| ChargeableWeightUnitCode | KG     |
		| GrossWeightUnitCode      | KG     |
		| DimensionsUnitCode       | Cm     |
		| VolumeUnitCode           | CBM    |
		| StatusCode               | CREA   |
	And an entry cross dock
	When create release cross dock
	Then the release cross dock should create successfully