Feature: External API – Direct Shipment
	With pre-prepared base and shipment data
	We want to create direct shipment with main carriage leg
	And update ETD, ATD, ETA, and ATA dates.

Background: Create direct shipment with main carriage leg
	Given a direct shipment with the following fields
		| Field                | Value      |
		| Agent                | 10026      |
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
	When create shipment
	Then shipment should create successfully

Scenario: Update main carriage ATA to future date
	When update ATA to future date
	Then should receive error message say cannot set main carriage ATA to future date

Scenario: Update main carriage ATD to future date
	When update ATD to future date
	Then should receive error message say cannot set main carriage ATD to future date

Scenario: Update main carriage ETD, ATD, ETA, and ATA to vaild dates
	When update ETD, ATD, ETA, and ATA to vaild dates
	Then shipment should update successfully