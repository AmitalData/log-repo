-- do not run online - already exist
update APInvoices set MasterNumber = ( select MasterShipmentNumber from ShipmentMasterDatas where id = MainEntityId)
update APInvoices set HouseNumber = ( select HouseNumber from Shipments where id = MainEntityId)


DECLARE @MainEntityId varchar(15) 
DECLARE @DirectionId varchar(15)
DECLARE @MainCarriageToPortId varchar(15)
DECLARE @MainCarriageFromPortId varchar(15)
DECLARE @value nvarchar(250)

DECLARE updatecursor CURSOR READ_ONLY
FOR
SELECT MainEntityId
FROM APInvoices

OPEN updatecursor

	FETCH NEXT FROM updatecursor
	INTO @MainEntityId


WHILE @@FETCH_STATUS = 0

BEGIN
  set @DirectionId = (select DirectionId from Shipments where id = @MainEntityId)
  set @MainCarriageFromPortId = ( select code from Ports where id = ( select MainCarriageFromPortId from ShipmentMasterDatas where id =@MainEntityId)) 
  set @MainCarriageToPortId =( select code from Ports where id = ( select MainCarriageToPortId from ShipmentMasterDatas where id =@MainEntityId)) 
  if ( @DirectionId = 'E')
  BEGIN
  set @value = 'Export To ' + @MainCarriageToPortId 

  END

   if ( @DirectionId = 'I')
  BEGIN
  set @value = 'Import From ' + @MainCarriageFromPortId 

  END

    if ( @DirectionId = 'D')
  BEGIN
  set @value = 'Ship to ' + @MainCarriageFromPortId 

  END

  update APInvoices set Description=@value
	where MainEntityId = @MainEntityId

	FETCH NEXT FROM updatecursor
	INTO @MainEntityId

END
print @value
CLOSE updatecursor
DEALLOCATE updatecursor


select * from APInvoices