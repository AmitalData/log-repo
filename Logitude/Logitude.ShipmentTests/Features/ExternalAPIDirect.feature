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
	And User adding main carriage legs to last direct shipmentd
		| LegIndex | Carrier.Code | FromPort.Code | ToPort.Code |
		| 1        | CA20         | DE222         | DE223       |
	When User create direct shipment using external API
	Then The direct shipment should be created successfully

Scenario: Update shipment with Invalid Future ATA
	Given The main carriage legs are added to last direct shipment
		| LegIndex | Carrier.Code | FromPort.Code | ToPort.Code |
		| 1        | CA20         | DE222         | DE223       |
	When User Update Shipment With Invalid Future ATA
	Then Update should not be done


Scenario: Update shipment with Invalid Future ATD
    Given The main carriage legs are added to last direct shipment
        | LegIndex | Carrier.Code | FromPort.Code | ToPort.Code |
        | 1        | CA20         | DE222         | DE223       |
    When The User Updates Shipment With Invalid Future ATD
    Then The excption massage that's related to this case is shown

Scenario:Update shipment with vaild dates
    Given The main carriage legs are added to last direct shipment
        | LegIndex | Carrier.Code | FromPort.Code | ToPort.Code |
        | 1        | CA20         | DE222         | DE223       |
    When The User Updates Shipment With valid Future ETD,ATD,ETA and ATA
    Then The shipment is updated succesfully


