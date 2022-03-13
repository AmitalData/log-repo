Feature: Run Report
	We want to run report.

Scenario: Run Report
	Given report with the following properties
		| property                 | Value  |
		| Direction                | Export |
		| TransportMode            | Air    |
		| ChargeableWeightUnitCode | KG     |
		| GrossWeightUnitCode      | KG     |
		| DimensionsUnitCode       | Cm     |
		| VolumeUnitCode           | CBM    |
		| StatusCode               | CREA   |
	And filters
		| Quantity | Length | Width | Height | Weight |
		| 70       | 10     | 20    | 30     | 40     |
		| 50       |        |       |        |        |
	When create entry cross dock
	Then the entry cross dock should create successfully