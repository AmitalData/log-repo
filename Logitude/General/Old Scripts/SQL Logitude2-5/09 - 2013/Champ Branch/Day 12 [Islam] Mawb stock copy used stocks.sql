 
begin transaction
begin
declare @Master as varchar(20)
declare @MainCarriageCarrierId as varchar(15)
declare @Tenant as int
declare @CurrentDate as datetime =  GETDATE()
declare @NewId as varchar(15)
  
BEGIN
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT [Master],MainCarriageCarrierId,Tenant
	FROM  ShipmentMasterDatas
	where MainCarriageIsFromStack = 'true' and [Master] <> NULL
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO  @Master,@MainCarriageCarrierId,@Tenant
	WHILE @@FETCH_STATUS = 0

		BEGIN
			if(Not Exists (select * from MAWBStacks where AirlineId = @MainCarriageCarrierId and Tenant = @Tenant and Number =  @Master))
			Begin
			print(@Master)
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'MAWBStack'
			INSERT INTO MAWBStacks(Id,Tenant,Number,AirlineId,InsertionDate,IsUsed)
	        VALUES (@NewId,@Tenant,@Master,@MainCarriageCarrierId,@CurrentDate,1)

			End
		


	FETCH NEXT FROM ShipmentsCursor  INTO  @Master,@MainCarriageCarrierId,@Tenant
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor	
END



 
END
commit transaction
