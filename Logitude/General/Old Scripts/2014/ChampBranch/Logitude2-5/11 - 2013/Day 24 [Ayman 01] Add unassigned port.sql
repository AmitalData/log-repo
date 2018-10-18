

--delete from Ports where Code = '---' and EnglishName = 'Unassigned'
--delete from Countries where Code = '--' and EnglishName = 'Unassigned'
--delete from GlobalZones where Code = '--' and EnglishName = 'Unassigned'

	BEGIN
		declare @Tenant as int
		declare @GlobalZoneId as varchar(15)
		declare @CountryId as varchar(15)
		declare @PortId as varchar(15)

		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				if NOT Exists (select * from GlobalZones where Code = '--' AND Tenant = @Tenant)
				begin
				EXECUTE usp_GetNextTableIdValue @GlobalZoneId OUTPUT,'GlobalZone'
				insert into GlobalZones(Id,Tenant,Code,EnglishName,LocalName,InActive,Notes,SearchFields)
				values (@GlobalZoneId,@Tenant,'--','Unassigned','Unassigned',1,null,'--,Unassigned,Unassigned')
				end

				else set @GlobalZoneId = (select Id from GlobalZones where Code = '--' AND Tenant = @Tenant)


				if NOT Exists (select * from Countries where Code = '--' AND Tenant = @Tenant)
				begin
				EXECUTE usp_GetNextTableIdValue @CountryId OUTPUT,'Country'
				insert into Countries(Id,Tenant,Code,EnglishName,LocalName,AddedManually,InActive,Notes,GlobalZoneId,EC,SearchFields)
				values (@CountryId,@Tenant,'--','Unassigned','Unassigned',1,1,null,@GlobalZoneId,0,'--,Unassigned,Unassigned')
				end

				else set @CountryId = (select Id from Countries where Code = '--' AND Tenant = @Tenant)

				if NOT Exists (select * from Ports where Code = '---' AND EnglishName = 'Unassigned' AND Tenant = @Tenant)
				begin
				EXECUTE usp_GetNextTableIdValue @PortId OUTPUT,'Port'
				insert into Ports(Id,Tenant,Code,EnglishName,LocalName,IsOcean,IsAir,IsInland,AddedManually,InActive,
				Notes,CountryId,Latitude,Longtitude,
				Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,
				SearchFields,StateId)
				
				values (
				@PortId,
				@Tenant,
				'---',
				'Unassigned',
				'Unassigned',
				0,
				1,
				0,
				1,
				1,
				null,
				@CountryId,
				0,0,
				null,null,null,null,null,null,null,null,null,null,
				'---,Unassigned,Unassigned',
				null
				)

				end

				else set @PortId = (select Id from Ports where Code = '---' AND Tenant = @Tenant)

			FETCH NEXT FROM TenantsCursor INTO @Tenant			
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
	END
