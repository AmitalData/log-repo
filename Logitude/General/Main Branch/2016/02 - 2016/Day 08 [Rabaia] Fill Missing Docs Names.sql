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
			
               UPDATE [dbo].[ShipmentComputedFields]
                 SET MissingDocumentsNames = (SELECT STUFF((
                 SELECT ',' + DocumentTypes.Name
                 FROM dbo.DocumentsFilings 
                 join dbo.Documents on dbo.Documents.Id = dbo.DocumentsFilings.DocumentId and dbo.Documents.Tenant = dbo.DocumentsFilings.Tenant
                 join dbo.DocumentTypes on  dbo.DocumentTypes.Id = DocumentTypeId and dbo.DocumentTypes.Tenant = dbo.DocumentsFilings.Tenant 
				 WHERE dbo.DocumentsFilings.Tenant = @Tenant AND EntityId = @Id and dbo.DocumentsFilings.ObjectTableId = (select Id from ObjectTables where Name='Shipment') AND DirectionCode = 'I' and 
                 ((dbo.DocumentTypes.Code = '740' OR dbo.DocumentTypes.Code = '706' OR dbo.DocumentTypes.Code = '380') AND dbo.Documents.HasFile = 0)
                 FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) 
                 where Id = @Id and Tenant = Tenant

		   FETCH NEXT FROM eventsCursor INTO @Id,@Tenant				
			END
		CLOSE eventsCursor
		DEALLOCATE eventsCursor
END