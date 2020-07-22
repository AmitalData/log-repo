
select Shipments.Id, Shipments.ComputedStatusId,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Id ELSE dbo.EntityStatus.Id END ELSE dbo.EntityStatus.Id END AS StatusId

INTO #newtable
from Shipments
LEFT OUTER JOIN   dbo.ShipmentMasterDatas ON dbo.ShipmentMasterDatas.Id = dbo.Shipments.MasterShipmentDataId 
LEFT OUTER JOIN dbo.EntityStatus AS ShipmentMasterDataEntityStatus ON dbo.ShipmentMasterDatas.StatusId = ShipmentMasterDataEntityStatus.Id 
 LEFT OUTER JOIN dbo.EntityStatus ON dbo.Shipments.StatusId = dbo.EntityStatus.Id

declare @ShipmentId as varchar(15)

	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Shipments
    WHERE id  in (select id from #newtable where ComputedStatusId !=StatusId)
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
  
	EXECUTE usp_ComputeShipmentStatus  @ShipmentId

	FETCH NEXT FROM ShipmentsCursor INTO  @ShipmentId
		End
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor

	drop table #newtable