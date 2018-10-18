


DECLARE @Tenant AS INT
declare @documenttypecopyid varchar(15)
declare @documenttypeid varchar(15)


	DECLARE TEUCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants
	
	OPEN TEUCursor FETCH NEXT FROM TEUCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
	print @Tenant
	set @documenttypeid=(select id from DocumentTypes where code='714' and Tenant=@Tenant)
	print @documenttypeid
	if(@documenttypeid is not null)
	begin
	EXECUTE usp_GetNextTableIdValue @documenttypecopyid OUTPUT,'DocumentTypeCopy'
	print @documenttypecopyid
    
	insert into DocumentTypeCopies (Id,Tenant,Code,Name,DocumentTypeId,IndexOrder,IsSelectedByDefault,InActive) values(@documenttypecopyid,@Tenant,'714C3','Copy 3',@documenttypeid,2,0,0)
	end
	FETCH NEXT FROM TEUCursor INTO @Tenant
	END
	CLOSE TEUCursor
	DEALLOCATE TEUCursor	




