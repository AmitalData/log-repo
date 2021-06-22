Feature: Create Cross Docks
	We want to create entries cross docks.

Scenario: Create cross dock
	Given a entries cross dock with the following properties
		| property                 | Value  |
		| Direction                | Export |
		| TransportMode            | Air    |
		| ChargeableWeightUnitCode | KG     |
		| GrossWeightUnitCode      | KG     |
		| DimensionsUnitCode       | Cm     |
		| VolumeUnitCode           | CBM    |
		| StatusCode               | CREA   |
	And a packages Details
		| Quantity | Length | Width | Height | Weight |
		| 70       | 10     | 20    | 30     | 40     |
		| 50       |        |       |        |        |
	When create cross dock
	Then the cross dock should create successfully