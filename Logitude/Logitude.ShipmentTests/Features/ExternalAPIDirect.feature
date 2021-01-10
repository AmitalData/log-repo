Feature: External API Direct

Background:
	Given Users with following credentials
		| Email              | Password    |
		| ahmadb123@mail.com | ahmed13!A15 |
	When Users make login request
	Then Users should have token

Scenario: Create Direct Shipment Using External API
	Given Direct shipment with the following properties
		| Name                      | Value      |
		| Agent.Code                | 10026      |
		| Direction.Code            | E          |
		| TransportMode.Code        | O          |
		| ShipmentType.Code         | FCLD       |
		| Shipper.Code              | 10009      |
		| ShipperReference1         | SR1        |
		| ShipperReference2         | SR2        |
		| GrossWeightUnit.Code      | KG         |
		| ChargeableWeightUnit.Code | KG         |
		| VolumeUnit.Code           | TES        |
		| Incoterm.Code             | CIF        |
		| MainCarriageCarrier.Code  | CA20       |
		| MainCarriageATD           | 2021-01-07 |
	And List of ocean or inland packages for direct shipment
		| PackageType.Code | Pieces | GrossWeight |
		| 20BU             | 1      | 250         |
	When User create direct shipment using external API
	Then The direct shipment should be created successfully

Scenario: xxxxxxxxxx1
	Given User adding main carriage legs to last direct shipment
		| LegIndex | Carrier.Code | FromPort.Code | ToPort.Code |
		| 1        | CA20         | DE222         | DE223       |
	When UpdateShipmentWithInvalidFutureATD
	Then assert

Scenario: xxxxxxxxxx2
	Given User adding main carriage legs to last direct shipment
		| LegIndex | Carrier.Code | FromPort.Code | ToPort.Code |
		| 1        | CA20         | DE222         | DE223       |
	When UpdateShipmentWithInvalidFutureATA
	Then assert ATA

Scenario: xxxxxxxxxx3
	Given User adding main carriage legs to last direct shipment
		| LegIndex | Carrier.Code | FromPort.Code | ToPort.Code |
		| 1        | CA20         | DE222         | DE223       |
	When UpdateShipmentWithValidDates
	Then assert *