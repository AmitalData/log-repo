
Declare @KeepValues as varchar(10) = '%[^0-9]%'
DECLARE @ComputedIATA VARCHAR(7)
DECLARE @ComputedCASS VARCHAR(4)

DECLARE @Tenant AS int
DECLARE @IATACode AS varchar(15)
DECLARE @CASSCode AS varchar(4)
DECLARE @ShipmentId AS varchar(15)

-- Loop Tenants
BEGIN
	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, IATA, CASSCode
	From Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant, @IATACode, @CASSCode
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @ComputedIATA = NULL
		set @ComputedCASS = NULL

		-- Trim everything that doesnt match '%[^0-9]%'
		While PatIndex(@KeepValues, @IATACode) > 0
		Set @IATACode = Stuff(@IATACode, PatIndex(@KeepValues, @IATACode), 1, '')
		
		While PatIndex(@KeepValues, @CASSCode) > 0
		Set @CASSCode = Stuff(@CASSCode, PatIndex(@KeepValues, @CASSCode), 1, '')

		if len(@CASSCode) > 0
		set @ComputedCASS = @CASSCode
		
		if len(@IATACode) > 0
		begin
			if len(@IATACode) <= 7
			set @ComputedIATA = @IATACode

			else
			begin
				set @ComputedIATA = left(@IATACode,7)

				if @ComputedCASS is null
				begin			
					set @ComputedCASS = SUBSTRING(@IATACode,8,len(@IATACode))

					if len(@ComputedCASS) > 4
					set @ComputedCASS = right(@ComputedCASS,4)
				end
			end
		end

		update Tenants
		set
		IATA = @ComputedIATA,
		CASSCode = @ComputedCASS
		where Id = @Tenant

	FETCH NEXT FROM TenantsCursor INTO @Tenant, @IATACode, @CASSCode
	END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END

-- Loop Shipments
BEGIN
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, IssuingCarrierIATACode, CASSCode
	From Shipments
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @IATACode, @CASSCode
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @ComputedIATA = NULL
		set @ComputedCASS = NULL

		-- Trim everything that doesnt match '%[^0-9]%'
		While PatIndex(@KeepValues, @IATACode) > 0
		Set @IATACode = Stuff(@IATACode, PatIndex(@KeepValues, @IATACode), 1, '')
		
		While PatIndex(@KeepValues, @CASSCode) > 0
		Set @CASSCode = Stuff(@CASSCode, PatIndex(@KeepValues, @CASSCode), 1, '')

		if len(@CASSCode) > 0
		set @ComputedCASS = @CASSCode
		
		if len(@IATACode) > 0
		begin
			if len(@IATACode) <= 7
			set @ComputedIATA = @IATACode

			else
			begin
				set @ComputedIATA = left(@IATACode,7)

				if @ComputedCASS is null
				begin			
					set @ComputedCASS = SUBSTRING(@IATACode,8,len(@IATACode))

					if len(@ComputedCASS) > 4
					set @ComputedCASS = right(@ComputedCASS,4)
				end
			end
		end

		update Shipments
		set
		IssuingCarrierIATACode = @ComputedIATA,
		CASSCode = @ComputedCASS
		where Tenant = @Tenant AND Id = @ShipmentId

	FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @IATACode, @CASSCode
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END


alter table Tenants alter column IATA varchar(7) null
go

alter table Shipments alter column IssuingCarrierIATACode varchar(7) null
go


update ObjectFields set MaxLength = 7, SystemMaxLength = 7 where FieldName = 'IATA' AND ObjectTableId = (Select Id from ObjectTables where Name = 'Tenant')
go

update ObjectFields set MaxLength = 7, SystemMaxLength = 7 where FieldName = 'IssuingCarrierIATACode' AND ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment')
go

update ObjectFields set MaxLength = 7 , SystemMaxLength = 7 where FieldName = 'IssuingCarrierIATACode' AND ObjectTableId = (Select Id from ObjectTables where Name = 'Master')
go