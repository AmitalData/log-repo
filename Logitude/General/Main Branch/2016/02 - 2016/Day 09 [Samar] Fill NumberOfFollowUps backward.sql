
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @FollowUpsCount as int

BEGIN 
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Shipments.Id, Shipments.Tenant
	FROM Shipments	
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant
	WHILE @@FETCH_STATUS = 0
		BEGIN

		set @FollowUpsCount = (select COUNT(*) from FollowUps where Tenant = @Tenant and ShipmentId = @ShipmentId)

		if(@FollowUpsCount > 0)
		begin

			update Shipments set NumberOfFollowUps = @FollowUpsCount where Id = @ShipmentId

		end

			FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END


