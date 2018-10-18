declare @Tenant as int
declare @Id as varchar(15)
 
BEGIN 
		DECLARE eventsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant
		FROM Shipments
    	OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@Tenant	
		WHILE @@FETCH_STATUS = 0
			BEGIN

		    insert into ShipmentAdditionalCloudDatas(Id,Tenant,IsImporterApprovalRequried) values(@Id,@Tenant,0)

		   FETCH NEXT FROM eventsCursor INTO @Id,@Tenant				
			END
		CLOSE eventsCursor
		DEALLOCATE eventsCursor
END