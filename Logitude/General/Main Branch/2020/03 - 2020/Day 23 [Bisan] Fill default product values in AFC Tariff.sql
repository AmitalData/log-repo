declare @Tenant as int
declare @EntityId as varchar(15)
declare @TariffProductId as varchar(15)

BEGIN
	DECLARE TariffProductsCursor CURSOR READ_ONLY
	FOR
		SELECT Id, Tenant
		FROM Tariffs
		WHERE TypeCode = 'AFC'
		OPEN TariffProductsCursor FETCH NEXT FROM TariffProductsCursor INTO @EntityId, @Tenant
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @TariffProductId = (SELECT Id FROM TariffProducts WHERE Tenant = @Tenant and Code = 'GEN')
			
			UPDATE Tariffs set TariffProductId = @TariffProductId WHERE Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM TariffProductsCursor INTO @EntityId, @Tenant
		END				
	CLOSE TariffProductsCursor
	DEALLOCATE TariffProductsCursor
END