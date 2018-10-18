
declare @ShipmentId as varchar(15)

	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Shipments
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	   begin
	
         EXECUTE usp_ComputeForeignPartnerCountryCode  @ShipmentId
			
		end

	FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId

	End
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
