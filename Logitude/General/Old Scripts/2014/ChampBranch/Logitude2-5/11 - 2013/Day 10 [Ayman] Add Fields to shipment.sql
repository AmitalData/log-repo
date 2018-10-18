
alter table Shipments add TransportDocumentNumber varchar(50) null
go

alter table Shipments add DeliveryOrder varchar(50) null
go

alter table Shipments add CarrierTransportDocumentNumber varchar(50) null
go

alter table Shipments add ImportManifest varchar(50) null
go

alter table Shipments add FreightLocationId varchar(15) null
go

alter table Shipments add constraint [Shipment_FreightLocationWarehouse] foreign key (FreightLocationId) references [Cards]([Id]);
go