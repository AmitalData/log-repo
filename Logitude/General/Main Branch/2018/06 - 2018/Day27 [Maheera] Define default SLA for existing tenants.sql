declare @Tenant as int
declare @DefaultId as varchAR(15)

BEGIN 
              DECLARE TenantsCursor CURSOR READ_ONLY
              FOR
              SELECT Id
              FROM Tenants
              OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant        
              WHILE @@FETCH_STATUS = 0
                     BEGIN
					 set @DefaultId = (select top(1) Id FROM SLAHeaders where Tenant = @Tenant)
		             update Tenants set DefaultSLAId = @DefaultId where Id = @Tenant 
                     FETCH NEXT FROM TenantsCursor INTO @Tenant      
                     END
              CLOSE TenantsCursor
              DEALLOCATE TenantsCursor
END