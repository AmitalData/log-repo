

alter table ShipmentPackages alter column Description nvarchar(250) null
go

alter table ShipmentPickUpDeliveryPackages add Width float null
go

alter table ShipmentPickUpDeliveryPackages add Length float null
go

alter table ShipmentPickUpDeliveryPackages add Height float null
go

