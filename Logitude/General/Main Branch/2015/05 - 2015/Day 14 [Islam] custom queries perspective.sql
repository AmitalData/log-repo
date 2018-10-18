--update Queries set Perspective = 

declare @Id as varchar(15)
declare @OriginalQueryId as varchar(15)
declare @Tenant as int
declare @perspective as varchar(25)

DECLARE QueriesCursor CURSOR READ_ONLY
       FOR    
       SELECT Id,OriginalQueryId,Tenant
       FROM Queries  
	   where OriginalQueryId is not null
       OPEN QueriesCursor FETCH NEXT FROM QueriesCursor INTO @Id,@OriginalQueryId,@Tenant
       WHILE @@FETCH_STATUS = 0
              BEGIN
           
		   set @perspective = (select Perspective from Queries where id = @OriginalQueryId )
		   update Queries set Perspective = @perspective where id = @Id 
              
			  print @perspective
              
        
       FETCH NEXT FROM QueriesCursor INTO @Id,@OriginalQueryId,@Tenant
       END
       CLOSE QueriesCursor
       DEALLOCATE QueriesCursor

