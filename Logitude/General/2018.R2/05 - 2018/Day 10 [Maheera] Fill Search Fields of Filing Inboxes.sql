
declare @Tenant as int
declare @EntityId as varchar(15)
declare @Subject as nvarchar(255)
declare @FileName as nvarchar(255)
declare @Description as nvarchar(max)
declare @MySearchFields as nvarchar(1000)

BEGIN
		DECLARE FilingInboxesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Subject
		FROM FilingInboxes
		OPEN FilingInboxesCursor FETCH NEXT FROM FilingInboxesCursor INTO @EntityId, @Tenant, @Subject
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MySearchFields = ''

			if (@Subject is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Subject
				else set @MySearchFields = @MySearchFields + ',' + @Subject	
			end
			
			DECLARE FilingInboxAttachmentsCursor CURSOR READ_ONLY
			FOR
			SELECT FileName
			FROM FilingInboxAttachments where FilingInboxId = @EntityId AND Tenant = @Tenant
			OPEN FilingInboxAttachmentsCursor FETCH NEXT FROM FilingInboxAttachmentsCursor INTO @FileName
			WHILE @@FETCH_STATUS = 0
			BEGIN

				if (@FileName is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @FileName
					else set @MySearchFields = @MySearchFields + ',' + @FileName	
				end
			FETCH NEXT FROM FilingInboxAttachmentsCursor  INTO @FileName

			END				
			CLOSE FilingInboxAttachmentsCursor
			DEALLOCATE FilingInboxAttachmentsCursor

			update FilingInboxes set SearchFields = @MySearchFields where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM FilingInboxesCursor INTO @EntityId, @Tenant, @Subject

		END				
		CLOSE FilingInboxesCursor
		DEALLOCATE FilingInboxesCursor
END


		