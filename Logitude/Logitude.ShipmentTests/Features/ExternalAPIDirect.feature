Feature: External API – Direct Shipment
	With pre-prepared base and shipment data
	We want to create direct shipment with main carriage leg
	And update ETD, ATD, ETA, and ATA dates.

Background: Create direct shipment with main carriage leg
	Given a direct shipment with the following fields
		| Field                | Value  |
		| Direction            | Export |
		| TransportMode        | Ocean  |
		| ShipmentType         | FCLD   |
		| ShipperReference1    | SR1    |
		| ShipperReference2    | SR2    |
		| GrossWeightUnit      | KG     |
		| ChargeableWeightUnit | KG     |
		| VolumeUnit           | TES    |
		| Incoterm             | LDE    |
		| MainCarriageCarrier  | MSCU   |
	And a main carriage leg
		| LegIndex | Carrier | FromPort | ToPort |
		| 1        | MSCU    | USNYC    | USSOU  |
	When create shipment
	Then shipment should create successfully
	# We don't need mapping for codes

Scenario: Update main carriage ATA to future date
	When update ATA to future date
	Then should receive error message say cannot set main carriage ATA to future date

Scenario: Update main carriage ATD to future date
	When update ATD to future date
	Then should receive error message say cannot set main carriage ATD to future date

Scenario: Update main carriage ETD, ATD, ETA, and ATA to vaild dates
	When update ETD, ATD, ETA, and ATA to vaild dates
	Then shipment should update successfully