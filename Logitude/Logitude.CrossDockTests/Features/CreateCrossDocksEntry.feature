Feature: Create Cross Docks Entry
	We want to create entry cross docks.

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