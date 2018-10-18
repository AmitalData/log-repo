declare @Tenant as int
declare @EntityId as varchar(15)
declare @ActivityEmailRecipientId as varchar(15)
declare @To as varchar(500)
declare @Cc as varchar(500)
declare @From as varchar(500)
declare @Email as varchar(70)
declare @RecipientTypeCode as varchar(3)
declare @SenderContactId as varchar(15)
declare @UserEmail as varchar(60)
declare @SenderEmail as varchar(60)

BEGIN
		DECLARE ActivitiesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, SenderContactId,SenderEmail
		FROM Activities
		WHERE ActivityTypeCode = 'EO' or ActivityTypeCode = 'EI'
		OPEN ActivitiesCursor FETCH NEXT FROM ActivitiesCursor INTO @EntityId, @Tenant, @SenderContactId, @SenderEmail
		WHILE @@FETCH_STATUS = 0
		BEGIN

			BEGIN
			set @UserEmail = null
			if (@SenderContactId is not null)
			begin
				if exists (select Email from Contacts where Tenant = @Tenant and Id = @SenderContactId and Email is not null)
					set @UserEmail = (select Email from Contacts where Tenant = @Tenant and Id = @SenderContactId)
			end
			if(@UserEmail is null) set @UserEmail = @SenderEmail
			END

			set @To = null
			set @Cc = null

			-- Loop ActivityEmailRecipients
			BEGIN
					DECLARE ActivityEmailRecipientsCursor CURSOR READ_ONLY
					FOR
					SELECT Id, Email,RecipientTypeCode
					FROM ActivityEmailRecipients
					WHERE Tenant = @Tenant AND ActivityId = @EntityId
					OPEN ActivityEmailRecipientsCursor FETCH NEXT FROM ActivityEmailRecipientsCursor INTO @ActivityEmailRecipientId, @Email, @RecipientTypeCode
					WHILE @@FETCH_STATUS = 0
					BEGIN

					if (@Email is not null and @RecipientTypeCode ='TO')
					begin
						if (@To is null) set @To = @Email	
						else if (CHARINDEX(@Email, @To) = 0) set @To = @To +';' + @Email	
					end

					if (@Email is not null and @RecipientTypeCode ='CC')
					begin
						if (@Cc is  null) set @Cc = @Email	
						else if (CHARINDEX(@Email, @Cc) = 0) set @Cc = @Cc +';' + @Email	
					end
					FETCH NEXT FROM ActivityEmailRecipientsCursor INTO @ActivityEmailRecipientId, @Email, @RecipientTypeCode

					END
					CLOSE ActivityEmailRecipientsCursor
					DEALLOCATE ActivityEmailRecipientsCursor
			END
		update Activities set [Cc] = RTRIM(LTRIM(@Cc)), [To] = RTRIM(LTRIM(@To)), [From] = @UserEmail  where Id = @EntityId AND Tenant = @Tenant
		FETCH NEXT FROM ActivitiesCursor INTO  @EntityId, @Tenant, @SenderContactId, @SenderEmail
		END				
		CLOSE ActivitiesCursor
		DEALLOCATE ActivitiesCursor
END
