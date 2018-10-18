declare @Tenant as int
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @ShipmentId as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @CountryForStatisticsId as varchar(15)
declare @DirectionId as varchar(1)
declare @TransmodeId as varchar(1)

BEGIN 
		DECLARE ShipmentsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant,FromPortId,ToPortId,ShipmentLevelCode,DirectionId,TransportModeId
		FROM Shipments
		OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId,@Tenant,@FromPortId,@ToPortId,@ShipmentLevelCode,@DirectionId,@TransmodeId
		WHILE @@FETCH_STATUS = 0
			BEGIN

		if(@ShipmentLevelCode = 'H')
		begin

		if(@DirectionId='I')
		begin

		set @CountryForStatisticsId=(select countryid from Ports where id=@FromPortId)
		
		end

		if(@DirectionId='E')
		begin

		set @CountryForStatisticsId=(select countryid from Ports where id=@ToPortId)

		end

	    if(@DirectionId='D' and @TransmodeId='A')
		begin

		set @CountryForStatisticsId=(select countryid from Ports where id=@ToPortId)

		end

		end

		else
		begin
			if(@DirectionId='I')
		begin

		set @FromPortId=(select MainCarriageFromPortId from ShipmentMasterDatas where Id=@ShipmentId)
		set @CountryForStatisticsId=(select countryid from Ports where id=@FromPortId)
		
		end

		if(@DirectionId='E')
		begin

		set @ToPortId=(select MainCarriageToPortId from ShipmentMasterDatas where Id=@ShipmentId)
		set @CountryForStatisticsId=(select countryid from Ports where id=@ToPortId)

		end

	    if(@DirectionId='D' and @TransmodeId='A')
		begin

		set @ToPortId=(select MainCarriageToPortId from ShipmentMasterDatas where Id=@ShipmentId)
		set @CountryForStatisticsId=(select countryid from Ports where id=@ToPortId)

		end
		end

		update Shipments set CountryForStatisticsId=@CountryForStatisticsId where Id=@ShipmentId and Tenant=@Tenant


				FETCH NEXT FROM ShipmentsCursor INTO  @ShipmentId,@Tenant,@FromPortId,@ToPortId,@ShipmentLevelCode,@DirectionId,@TransmodeId
			END
		CLOSE ShipmentsCursor
		DEALLOCATE ShipmentsCursor
END