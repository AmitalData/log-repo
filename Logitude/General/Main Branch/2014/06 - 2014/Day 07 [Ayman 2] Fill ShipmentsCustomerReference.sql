
update Shipments set
CustomerReference1 = null,
CustomerReference2 = null
where ShipmentLevelCode = 'C'
go

update Shipments set
CustomerReference1 = ShipperReference1,
CustomerReference2 = ShipperReference2
where ShipmentCustomerTypeCode = 'SHI'
AND ShipmentLevelCode != 'C'
go

update Shipments set
CustomerReference1 = ConsigneeReference1,
CustomerReference2 = ConsigneeReference2
where ShipmentCustomerTypeCode = 'CON'
AND ShipmentLevelCode != 'C'
go

update Shipments set
CustomerReference1 = AgentReference1,
CustomerReference2 = AgentReference2
where ShipmentCustomerTypeCode = 'OTH'
AND ShipmentLevelCode != 'C'
AND CustomerId = AgentId
go

update Shipments set
CustomerReference1 = CustomAgentExportReference,
CustomerReference2 = null
where ShipmentCustomerTypeCode = 'OTH'
AND ShipmentLevelCode != 'C'
AND CustomerId = CustomAgentExportId
go

update Shipments set
CustomerReference1 = CustomAgentImportReference,
CustomerReference2 = null
where ShipmentCustomerTypeCode = 'OTH'
AND ShipmentLevelCode != 'C'
AND CustomerId = CustomAgentImportId
go

update Shipments set
CustomerReference1 = FreightForwarderReference,
CustomerReference2 = null
where ShipmentCustomerTypeCode = 'OTH'
AND ShipmentLevelCode != 'C'
AND CustomerId = FreightForwarderId
go

update Shipments set
CustomerReference1 = ColoaderReference1,
CustomerReference2 = null
where ShipmentCustomerTypeCode = 'OTH'
AND ShipmentLevelCode != 'C'
AND CustomerId = ColoaderId
go

update Shipments set
CustomerReference1 = CustomClearancePointReference1,
CustomerReference2 = null
where ShipmentCustomerTypeCode = 'OTH'
AND ShipmentLevelCode != 'C'
AND CustomerId = CustomClearancePointId
go