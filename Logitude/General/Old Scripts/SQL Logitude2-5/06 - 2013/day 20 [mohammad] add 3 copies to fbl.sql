-- do not run online - already exist
DECLARE @Tenant AS INT
declare @documenttypecopyid varchar(15)
declare  @documenttypecopyid2 varchar(15)
declare  @documenttypecopyid3 varchar(15)
declare @documenttypeid varchar(15)


	DECLARE TEUCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants
	
	OPEN TEUCursor FETCH NEXT FROM TEUCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
	print @Tenant
	set @documenttypeid=(select id from DocumentTypes where code='716' and Tenant=@Tenant)
	print @documenttypeid
	if(@documenttypeid is not null)
	begin
	EXECUTE usp_GetNextTableIdValue @documenttypecopyid OUTPUT,'DocumentTypeCopy'
	print @documenttypecopyid

	EXECUTE usp_GetNextTableIdValue @documenttypecopyid2 OUTPUT,'DocumentTypeCopy'
	print @documenttypecopyid2

	EXECUTE usp_GetNextTableIdValue @documenttypecopyid3 OUTPUT,'DocumentTypeCopy'
	print @documenttypecopyid3
    
	insert into DocumentTypeCopies (Id,Tenant,Code,Name,DocumentTypeId,IndexOrder,IsSelectedByDefault,InActive) values(@documenttypecopyid,@Tenant,'714OR','Original',@documenttypeid,2,0,0)
	insert into DocumentTypeCopies (Id,Tenant,Code,Name,DocumentTypeId,IndexOrder,IsSelectedByDefault,InActive) values(@documenttypecopyid2,@Tenant,'714CN','Copy Not Negotiable',@documenttypeid,2,0,0)
	insert into DocumentTypeCopies (Id,Tenant,Code,Name,DocumentTypeId,IndexOrder,IsSelectedByDefault,InActive) values(@documenttypecopyid3,@Tenant,'714ER','Express Release',@documenttypeid,2,0,0)

	end
	FETCH NEXT FROM TEUCursor INTO @Tenant
	END
	CLOSE TEUCursor
	DEALLOCATE TEUCursor	