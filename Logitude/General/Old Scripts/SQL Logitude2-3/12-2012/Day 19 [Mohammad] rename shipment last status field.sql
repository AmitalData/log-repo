sp_RENAME'Shipments.[ShipmentLastStatusCode]' , 'CarrierLastStatusCode', 'COLUMN'
go
sp_RENAME 'Shipments.[ShipmentLastStatusUpdate]' , 'CarrierLastStatusDate', 'COLUMN'
go

drop index [IX_FK_ShipmentAWBStatus] on Shipments
go

-- Creating non-clustered index for FOREIGN KEY 'FK_ShipmentAWBStatus'
CREATE INDEX [IX_FK_ShipmentAWBStatus]
ON [dbo].[Shipments]
    ([CarrierLastStatusCode]);
GO

update objectfields set fieldname='CarrierLastStatusName'
where fieldname='ShipmentLastStatusName'
go

update objectfields set fieldname='CarrierLastStatusCode'
where fieldname='ShipmentLastStatusCode'
go

update objectfields set fieldname='CarrierLastStatusDate'
where fieldname='ShipmentLastStatusUpdate'
go

update textcodes set code='Shipment.F.CarrierLastStatusCode'
where code ='Shipment.F.ShipmentLastStatusCode'
go

update textcodes set code='Shipment.CarrierLastStatusCodeHelpText'
where code ='Shipment.ShipmentLastStatusCodeHelpText'
go

update textcodes set code='Shipment.F.CarrierLastStatusName'
where code ='Shipment.F.ShipmentLastStatusName'
go

update textcodes set code='Shipment.CarrierLastStatusNameHelpText'
where code ='Shipment.ShipmentLastStatusNameHelpText'
go

update textcodes set code='Shipment.F.CarrierLastStatusName'
where code ='Shipment.F.ShipmentLastStatusName'
go

update textcodes set code='Shipment.CH.CarrierLastStatusNameListLable'
where code ='Shipment.CH.ShipmentLastStatusNameListLable'
go

update textcodes set defaulttext='Last Status Date'
where code ='Shipment.F.ShipmentLastStatusUpdate'
go

update textcodes set code='Shipment.F.CarrierLastStatusDate'
where code ='Shipment.F.ShipmentLastStatusUpdate'
go

update textcodes set code='Shipment.CarrierLastStatusDateHelpText'
where code ='Shipment.ShipmentLastStatusUpdateHelpText'
go

update textcodes set defaulttext='Last Status Date'
where code ='Shipment.CH.ShipmentLastStatusUpdateListLable'
go

update textcodes set code='Shipment.CH.CarrierLastStatusDateListLable'
where code ='Shipment.CH.ShipmentLastStatusUpdateListLable'
go
