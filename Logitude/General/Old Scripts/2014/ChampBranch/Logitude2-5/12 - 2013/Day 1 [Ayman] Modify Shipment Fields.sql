
---- No need to execute on Main
-------------------------------


--if exists(select * from sys.columns where Name = 'ImportManifest' and Object_ID = Object_ID('Shipments'))
--alter table Shipments drop column ImportManifest
--go

--if exists(select * from sys.columns where Name = 'TransportDocumentNumber' and Object_ID = Object_ID('Shipments'))
--alter table Shipments drop column TransportDocumentNumber
--go

--if not exists(select * from sys.columns where Name = 'ImportManifest' and Object_ID = Object_ID('ShipmentMasterDatas'))
--alter table ShipmentMasterDatas add ImportManifest varchar(50)
--go

--if not exists(select * from sys.columns where Name = 'TransportDocumentNumber' and Object_ID = Object_ID('ShipmentMasterDatas'))
--alter table ShipmentMasterDatas add TransportDocumentNumber varchar(50)
--go