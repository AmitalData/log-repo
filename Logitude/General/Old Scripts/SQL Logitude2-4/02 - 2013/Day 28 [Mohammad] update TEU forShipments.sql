-- later
-- Run it in the last
-- Script To Update All AR Invoices Transfer fields --

DECLARE @Tenant AS INT
DECLARE @ShipmentId AS varchar(15)
DECLARE @PackageTypeId AS varchar(15)

BEGIN;
	DECLARE TEUCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant,Id
	FROM Shipments
	OPEN TEUCursor FETCH NEXT FROM TEUCursor INTO @Tenant,@ShipmentId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	Declare @TEUSumForShipment AS float
	set @TEUSumForShipment=0
	DECLARE TEUPackageCursor CURSOR READ_ONLY
	FOR	
	SELECT PackageTypeId
	FROM ShipmentPackages
	where ShipmentId=@ShipmentId
	OPEN TEUPackageCursor FETCH NEXT FROM TEUPackageCursor INTO @PackageTypeId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	print ('in package cursor')
	Declare @TEU AS float
	set @TEU=(select TEU from PackageTypes
	where Id=@PackageTypeId and IsContainer=1)
	print ('teu for' + @PackageTypeId)
	print (@TEU)
	set @TEUSumForShipment=@TEUSumForShipment+@TEU
	print('teu total:')
	print(@TEUSumForShipment)
	FETCH NEXT FROM TEUPackageCursor INTO @PackageTypeId
	END
	CLOSE TEUPackageCursor
	DEALLOCATE TEUPackageCursor

	print ('in shipment cursor')
	update shipments set TEU=@TEUSumForShipment
	where Id=@ShipmentId
	print(@ShipmentId)
	print(@TEUSumForShipment)
	FETCH NEXT FROM TEUCursor INTO @Tenant,@ShipmentId
	END
	CLOSE TEUCursor
	DEALLOCATE TEUCursor	
END