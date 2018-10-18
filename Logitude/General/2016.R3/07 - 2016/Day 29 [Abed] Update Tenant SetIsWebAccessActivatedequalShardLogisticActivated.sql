

declare @IsSharedLogisticsActivated  as bit
declare @Id as varchar(15)
	DECLARE TenantCursor CURSOR READ_ONLY
	FOR
	SELECT Id,IsSharedLogisticsActivated
	From Tenants
  
	OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Id,@IsSharedLogisticsActivated
	WHILE @@FETCH_STATUS = 0
	BEGIN

	   begin
	


       update Tenants set IsWebAccessActivated = @IsSharedLogisticsActivated   where Id = @Id 
		end

	FETCH NEXT FROM TenantCursor INTO  @Id,@IsSharedLogisticsActivated

	End
	CLOSE TenantCursor
	DEALLOCATE TenantCursor
