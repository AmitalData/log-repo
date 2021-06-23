@Pre-Prepare-release
Feature: Create Cross Docks
    With pre-prepared entry data
	We want to create entry and release cross docks.

Scenario: Create entry cross dock
	Given an entry cross dock with the following properties
		| property                 | Value  |
		| Direction                | Export |
		| TransportMode            | Air    |
		| ChargeableWeightUnitCode | KG     |
		| GrossWeightUnitCode      | KG     |
		| DimensionsUnitCode       | Cm     |
		| VolumeUnitCode           | CBM    |
		| StatusCode               | CREA   |
	And packages details
		| Quantity | Length | Width | Height | Weight |
		| 70       | 10     | 20    | 30     | 40     |
		| 50       |        |       |        |        |
	When create entry cross dock
	Then the entry cross dock should create successfully

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