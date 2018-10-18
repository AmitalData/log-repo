DECLARE @tenant AS INT
declare @contactid varchar(15)
BEGIN;
	DECLARE TEUCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,Tenant
	FROM Contacts
	OPEN TEUCursor FETCH NEXT FROM TEUCursor INTO @contactid,@tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	declare @contacttenantid varchar(15)
	set @contacttenantid=(select id from ContactTenants where ContactId=@contactid and TenantId=@tenant)
	if (@contacttenantid is null)
	begin
	print 'adding contact tenant for:'
	print @contactid

	EXECUTE usp_GetNextTableIdValue @contacttenantid OUTPUT,'ContactTenant'
	print @contacttenantid
	insert into ContactTenants (Id,ContactId,TenantId) values (@contacttenantid,@contactid,@tenant)


	end

	FETCH NEXT FROM TEUCursor INTO @contactid,@tenant
	END
	CLOSE TEUCursor
	DEALLOCATE TEUCursor	
END


select * from contacts where id not in (select ContactTenants.contactid from ContactTenants where ContactTenants.TenantId=1) and Tenant=1

