Feature: External API Direct

Background:
	Given Direct shipment with the following properties
		| Name                 | Value      |
		| Agent                | 10026      |
		| Direction            | E          |
		| TransportMode        | O          |
		| ShipmentType         | FCLD       |
		| Shipper              | 10009      |
		| ShipperReference1    | SR1        |
		| ShipperReference2    | SR2        |
		| GrossWeightUnit      | KG         |
		| ChargeableWeightUnit | KG         |
		| VolumeUnit           | TES        |
		| Incoterm             | CIF        |
		| MainCarriageCarrier  | CA20       |
		| MainCarriageATD      | 2021-01-07 |
	And List of main carriage legs
		| LegIndex | Carrier | FromPort | ToPort |
		| 1        | CA20    | DE222    | DE223  |
	When Create direct shipment using external API
	Then The direct shipment should be created successfully

Scenario: Update shipment main carriage leg with invalid ATA
	When Update main carriage leg ATA to future date
	Then Error message (cannot set main carriage ATA to future date) should received

Scenario: Update shipment main carriage leg with invalid ATD
	When Update main carriage leg ATD to future date
	Then Error message (cannot set main carriage ATD to future date) should received

Scenario: Update shipment main carriage leg with vaild dates
	When Update main carriage leg ETD,ATD,ETA and ATA to valid date
	Then The shipment should updated succesfully