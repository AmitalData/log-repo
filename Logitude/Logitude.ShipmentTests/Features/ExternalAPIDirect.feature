Feature: External API – Direct
	With pre-prepared base and shipment data
	We want to create direct shipment with main carriage leg
	And update ETD,ATD,ETA and ATA dates.

Background: Create direct shipment with main carriage leg
	Given a direct shipment with the following properties
		| property             | Value      |
		| Agent				   | 10026      |
		| Direction            | Export     |
		| TransportMode        | Ocean      |
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
	And a main carriage leg
		| LegIndex | Carrier | FromPort | ToPort |
		| 1        | CA20    | DE222    | DE223  |
	When create direct shipment
	Then the direct should create successfully

Scenario: Set main carriage ATA to future date
	When set main carriage ATA to future date
	Then ATA error message should received

Scenario: Set main carriage ATD to future date
	When set main carriage ATD to future date
	Then ATD error message should received

Scenario: Set main carriage ETD,ATD,ETA and ATA to vaild dates
	When Set main carriage ETD,ATD,ETA and ATA to vaild dates
	Then the direct should add update successfully