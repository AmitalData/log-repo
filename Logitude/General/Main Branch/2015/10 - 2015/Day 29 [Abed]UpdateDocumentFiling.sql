
begin

declare @DocCounterId as varchar(15)
declare @DocLastNumber as int
declare @Tenant as int

       DECLARE TenantCursor CURSOR READ_ONLY
       FOR
       SELECT Id
       From Tenants 
       OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
       WHILE @@FETCH_STATUS = 0
     
 BEGIN
   

                set @DocLastNumber = (SELECT LastNumber
                FROM CounterLastNumbers with (UPDLOCK) WHERE TableName ='DocumentsFiling' AND Tenant = @Tenant)


		        set @DocCounterId = ( select id from Counters where ObjectTableId = (select id from ObjectTables where Name = 'documentsfiling'))



               if (@DocCounterId is not null and  @DocLastNumber is not null)

			    BEGIN

                IF NOT EXISTS (SELECT CounterId
                FROM CounterStats (UPDLOCK) WHERE CounterId = @DocCounterId and Tenant =  @Tenant)
		 
		        begin
			
                INSERT INTO CounterStats(Tenant,CounterId,Prefix,LastValue)
                VALUES ( @Tenant, @DocCounterId,'', @DocLastNumber + 1)
				
	            end

				End

				

       FETCH NEXT FROM TenantCursor INTO @Tenant
  END 

CLOSE TenantCursor
DEALLOCATE TenantCursor


  End

