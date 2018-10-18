
		 

declare @Id as varchar(15)
declare @Tenant as int
 
BEGIN
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant
	FROM  Shipments
	where  SecurityKey IS  NULL
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @Id,@Tenant
	WHILE @@FETCH_STATUS = 0

		BEGIN
		
DECLARE @guidNew varchar(40)
SET @guidNew = replace(newid(), '-', '') 
PRINT 'Value of @guidNew is: '+ @guidNew
 
		 
	update  shipments set SecurityKey = @guidNew where Id = @Id and Tenant = @Tenant

	FETCH NEXT FROM ShipmentsCursor INTO @Id,@Tenant
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor	
END




