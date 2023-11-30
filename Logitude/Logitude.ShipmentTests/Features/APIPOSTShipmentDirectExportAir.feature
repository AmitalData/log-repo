Feature: POST Shipment Direct Export Air
	The API creates a Direct Export Air shipment.
@Smoke
@Release 
Scenario: POST Shipment Direct Export Air
	Given a direct shipment with the following properties
		| property              | Value   |
		| Direction             | Export  |
		| TransportMode         | Air     |
		| ShipmentLevel         | Direct  |
		| FreightPrepaidCollect | Collect |
		| OtherPrepaidCollect   | Collect |
		| MainCarriageToPort    | JFK     |
		| MainCarriageFromPort  | MIA     |
	When create direct shipment
	Then the direct should create successfully

