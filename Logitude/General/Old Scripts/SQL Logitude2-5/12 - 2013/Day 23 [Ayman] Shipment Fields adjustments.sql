
if not exists(select * from sys.columns where Name = 'ImportManifest' and Object_ID = Object_ID('ShipmentMasterDatas'))
alter table ShipmentMasterDatas add ImportManifest varchar(50) null
go

if not exists(select * from sys.columns where Name = 'CarrierTransportDocumentNumber' and Object_ID = Object_ID('ShipmentMasterDatas'))
alter table ShipmentMasterDatas add CarrierTransportDocumentNumber varchar(50) null
go

if exists(select * from sys.columns where Name = 'ImportManifest' and Object_ID = Object_ID('Shipments'))
	BEGIN
		declare @Tenant as int
		declare @EntityId as varchar(15)
		declare @ImportManifest as varchar(50)
		declare @CarrierTransportDocumentNumber as varchar(50)

		DECLARE ShipmentsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant,ImportManifest,CarrierTransportDocumentNumber
		FROM Shipments
		OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @EntityId,@Tenant,@ImportManifest,@CarrierTransportDocumentNumber
		WHILE @@FETCH_STATUS = 0
			BEGIN
				print 'ddd3'
				update ShipmentMasterDatas
				set ImportManifest = @ImportManifest,
				CarrierTransportDocumentNumber = @CarrierTransportDocumentNumber
				where Id = @EntityId AND Tenant = @Tenant

			FETCH NEXT FROM ShipmentsCursor INTO @EntityId,@Tenant,@ImportManifest,@CarrierTransportDocumentNumber	
			END
		CLOSE ShipmentsCursor
		DEALLOCATE ShipmentsCursor
	END
go


--if exists(select * from sys.columns where Name = 'ImportManifest' and Object_ID = Object_ID('Shipments'))
--alter table Shipments drop column ImportManifest
--go

--if exists(select * from sys.columns where Name = 'CarrierTransportDocumentNumber' and Object_ID = Object_ID('Shipments'))
--alter table Shipments drop column CarrierTransportDocumentNumber
--go
