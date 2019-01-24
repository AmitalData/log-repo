declare @Tenant as int
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @ShipmentId as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @CountryForStatisticsId as varchar(15)
declare @DirectionId as varchar(1)
declare @TransmodeId as varchar(1)
declare @MainCarriageToAddressId as varchar(15)
declare @ToAddressCountryId as varchar(15)
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

			    if(@DirectionId='D' and @TransmodeId='O')
		begin

		set @CountryForStatisticsId=(select countryid from Ports where id=@ToPortId)

		end

		if(@DirectionId='D' and @TransmodeId='I')
		begin
		set @MainCarriageToAddressId= (select MainCarriageToAddressId from ShipmentMasterDatas where Id=@ShipmentId and Tenant=@Tenant  )
		if(@MainCarriageToAddressId is not null)
		begin
		set @ToAddressCountryId=(SELECT Countries.Id
FROM Countries
INNER JOIN Addresses ON Countries.Id=Addresses.CountryId where Countries.Tenant=@Tenant and Addresses.Id=@MainCarriageToAddressId)
if(@ToAddressCountryId is not null)
		begin
				set @CountryForStatisticsId=@ToAddressCountryId

		end


		end

		end

			if(@DirectionId='C')
		begin

		set @CountryForStatisticsId=(select countryid from Ports where id=@FromPortId)

		end

		if(@DirectionId='R')
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
		
			    if(@DirectionId='D' and @TransmodeId='O')
		begin

		set @CountryForStatisticsId=(select countryid from Ports where id=@ToPortId)

		end

				if(@DirectionId='D' and @TransmodeId='I')
		begin
		set @MainCarriageToAddressId= (select MainCarriageToAddressId from ShipmentMasterDatas where Id=@ShipmentId and Tenant=@Tenant  )
		if(@MainCarriageToAddressId is not null)
		begin
		set @ToAddressCountryId=(SELECT Countries.Id
FROM Countries
INNER JOIN Addresses ON Countries.Id=Addresses.CountryId where Countries.Tenant=@Tenant and Addresses.Id=@MainCarriageToAddressId)
if(@ToAddressCountryId is not null)
		begin
				set @CountryForStatisticsId=@ToAddressCountryId

		end


		end

		end

			if(@DirectionId='C')
		begin

		set @CountryForStatisticsId=(select countryid from Ports where id=@FromPortId)

		end

		if(@DirectionId='R')
		begin

		set @CountryForStatisticsId=(select countryid from Ports where id=@ToPortId)

		end
		end

		update Shipments set CountryForStatisticsId=@CountryForStatisticsId where Id=@ShipmentId and Tenant=@Tenant

				FETCH NEXT FROM ShipmentsCursor INTO  @ShipmentId,@Tenant,@FromPortId,@ToPortId,@ShipmentLevelCode,@DirectionId,@TransmodeId
			END
		CLOSE ShipmentsCursor
		DEALLOCATE ShipmentsCursor
END