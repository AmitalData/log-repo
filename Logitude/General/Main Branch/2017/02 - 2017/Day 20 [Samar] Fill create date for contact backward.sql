
declare @CreateContactEventTypeId as varchar(15)
declare @CreateDate as datetime
declare @ContactId as varchar(15)
declare @Tenant as int
declare @TableId as varchar(15)
set @TableId = (select Id from ObjectTables where Name = 'Contact')

BEGIN 
	DECLARE ContactsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	FROM Contacts
	OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO @ContactId, @Tenant		
	WHILE @@FETCH_STATUS = 0
		BEGIN
			
			set @CreateContactEventTypeId = (select Id from EventTypes where Tenant = @Tenant and Code = 'CRCO' and ObjectTableId = @TableId)
			set @CreateDate = (select MAX(LogDateTime) from TraceEvents where Tenant = @Tenant and EntityId = @ContactId and ObjectTableId = @TableId and EventTypeId = @CreateContactEventTypeId)										
			
			update Contacts
			set	CreateDate = @CreateDate
			where Id = @ContactId				

			FETCH NEXT FROM ContactsCursor INTO @ContactId,  @Tenant
		END

	CLOSE ContactsCursor
	DEALLOCATE ContactsCursor
END