declare @Tenant as int
declare @Id as varchar(15)
declare @TempString as varchar(15)
DECLARE @temp TABLE (Fields NVARCHAR(max))

BEGIN 
		DECLARE eventsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant
		FROM Shipments
    	OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@Tenant	
		WHILE @@FETCH_STATUS = 0
			BEGIN
               
               delete from @temp
               INSERT INTO @temp (Fields) 
               SELECT SearchFields FROM dbo.DocumentsFilings 
               WHERE Tenant = @Tenant AND EntityId = @Id and ObjectTableId = (select Id from ObjectTables where Name='Shipment') AND DirectionCode = 'I' and SearchFields is not null and SearchFields <> ''
               
                 UPDATE [dbo].[ShipmentComputedFields]
                 SET [DocumentsSearchFields] = (SELECT STUFF((
                 SELECT ',' + Fields
                 FROM @temp
                 FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''))
                 where Id = @Id and ((select COUNT(*) from @temp) > 0)
             

		   FETCH NEXT FROM eventsCursor INTO @Id,@Tenant				
			END
		CLOSE eventsCursor
		DEALLOCATE eventsCursor
END