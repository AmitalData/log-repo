
declare @ShipmentId as varchar(15)

	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Shipments
    WHERE StatusDate >= DATEADD(DAY, -30, GETDATE()) 
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
  
	EXECUTE usp_ComputeShipmentStatus  @ShipmentId

	FETCH NEXT FROM ShipmentsCursor INTO  @ShipmentId
		End
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
