Feature: GET Shipment Direct Export Air
	The API gets a Direct Export Air shipment.

Scenario: GET Shipment Direct Export Air
	When get shipment with shipmentnumber
	Then shipment should be avaliable