
declare @FollowUpId as varchar(15)
declare @InternalDocumentId as varchar(40)
declare @Tenant as int
declare @DocumentsFilingId as varchar(40)
declare @DocumentTypeId as varchar(15)
declare @Area as varchar(20)


	DECLARE FollowUpCursor CURSOR READ_ONLY
	FOR
	SELECT Id , Tenant, DocumentsFilingId , InternalDocumentId
	From FollowUps
	where (DocumentsFilingId is not null or InternalDocumentId is not null) and DocumentTypeId is null

	OPEN FollowUpCursor FETCH NEXT FROM FollowUpCursor INTO @FollowUpId , @Tenant, @DocumentsFilingId , @InternalDocumentId
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		if(@DocumentsFilingId is not null)
	 begin
	 set @DocumentTypeId = (select DocumentTypeId from DocumentsFilings where id = @DocumentsFilingId AND Tenant = @Tenant )
	 	  set @Area  = 'DocIn'
	end

	if(@InternalDocumentId is not null)
	 begin
	  set @DocumentTypeId = (select DocumentTypeId from DocumentsFilings where id = @InternalDocumentId AND Tenant = @Tenant )
	  set @Area  = 'DocOut'
	end



	if(@DocumentTypeId is not null)
	   begin
	      update FollowUps set DocumentTypeId = @DocumentTypeId , Area =@Area where ID = @FollowUpId and Tenant = @Tenant
		end


	FETCH NEXT FROM FollowUpCursor INTO @FollowUpId , @Tenant, @DocumentsFilingId , @InternalDocumentId

	End
	CLOSE FollowUpCursor
	DEALLOCATE FollowUpCursor
