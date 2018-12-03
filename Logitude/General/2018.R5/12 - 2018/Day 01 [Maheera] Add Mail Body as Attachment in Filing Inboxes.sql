-- Run it Once 

declare @Tenant as int
declare @Id as varchar(15)
declare @BodyDocumentId as varchar(15)
declare @BodyDocumentId_Check as varchar(15)
declare @NewId as varchar(15)

BEGIN
	DECLARE FilingInboxCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, BodyDocumentId
	FROM FilingInboxes
	OPEN FilingInboxCursor FETCH NEXT FROM FilingInboxCursor INTO @Id , @Tenant, @BodyDocumentId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		begin
					set @BodyDocumentId_Check = (select Id from FilingInboxAttachments where Tenant = @Tenant and DocumentId = @BodyDocumentId)
					if (@BodyDocumentId_Check is null)
					begin
						EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'FilingInboxAttachment'
						insert into FilingInboxAttachments(Id, Tenant, FileName, DocumentId, FilingInboxId)
						values( @NewId, @Tenant, 'Mail Body', @BodyDocumentId, @Id)
				    end
		end
	FETCH NEXT FROM FilingInboxCursor INTO  @Id , @Tenant, @BodyDocumentId
	END
	CLOSE FilingInboxCursor
	DEALLOCATE FilingInboxCursor
END