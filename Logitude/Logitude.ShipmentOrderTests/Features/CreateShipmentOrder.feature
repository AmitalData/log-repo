Feature: Create Shipment Order
	We want to create Shipment Order.

Scenario: Create shipment order
	Given a shipment order with the following properties
		| property           | Value                       |
		| Direction          | Export                      |
		| TransportMode      | Air                         |
		| PONumber           | 12345                       |
		| DescriptionOfGoods | specflow description        |
		| CustomerReferences | specflow references         |
		| ShipmentNumber     | 12345                       |
		| Shipper            | TestShipperExport           |
		| Agent              | TestAgentExport             |
		| Incoterm           | LDE Incoterm                |
		| OriginPort         | John F.Kennedy Apt/New York |
		| CreateDate         | Today                       |
	When create shipment order
	Then the shipment order should create successfully