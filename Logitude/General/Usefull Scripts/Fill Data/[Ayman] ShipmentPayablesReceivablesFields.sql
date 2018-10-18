
declare @ShipmentId as varchar(15)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Shipments
	where ShipmentLevelCode = 'D'
	--where ShipmentLevelCode = 'C'
	--where ShipmentLevelCode = 'H' AND MasterShipmentDataId is null
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @ShipmentId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		EXECUTE usp_UpdateShipmentProfit @ShipmentId

	FETCH NEXT FROM DataCursor INTO @ShipmentId
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END

-- 22/02/2018 (Pre)
--select count(*) from shipments where ShipmentLevelCode = 'H' and MasterShipmentDataId is null		(15059)		(00:04:52)
--select count(*) from shipments where ShipmentLevelCode = 'D'										(355864)	(00:47:22)
--select count(*) from shipments where ShipmentLevelCode = 'C'										(75296)		(00:46:49)
--select count(*) from shipments where ShipmentLevelCode = 'H' and MasterShipmentDataId is not null	(149832)


--24/08/2017 (Production)
--select count(*) from shipments where ShipmentLevelCode = 'H' and MasterShipmentDataId is null		(11758)		(00:00:40)
--select count(*) from shipments where ShipmentLevelCode = 'D'										(292620)	(00:19:48)
--select count(*) from shipments where ShipmentLevelCode = 'C'										(61365)		(00:16:19)
--select count(*) from shipments where ShipmentLevelCode = 'H' and MasterShipmentDataId is not null	(125663)
