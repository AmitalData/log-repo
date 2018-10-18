
declare @Tenant as int
declare @EntityId as varchar(15)
declare @BookingConfirmationNumber as nvarchar(25)
declare @MySearchFields as nvarchar(max)

BEGIN
		DECLARE ShipmentsCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, BookingConfirmationNumber
		FROM ShipmentMasterDatas
		OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @BookingConfirmationNumber
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MySearchFields = ''

			if (@BookingConfirmationNumber is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @BookingConfirmationNumber
				else set @MySearchFields = @MySearchFields + ',' + @BookingConfirmationNumber	
			end
				
			update Shipments set SearchFields = @MySearchFields where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @BookingConfirmationNumber

		END				
		CLOSE ShipmentsCursor
		DEALLOCATE ShipmentsCursor
END