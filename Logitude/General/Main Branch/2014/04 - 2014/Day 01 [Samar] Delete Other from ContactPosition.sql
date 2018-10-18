
declare @Tenant as int
declare @PositionId as varchar(15)

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @PositionId = (select Id from ContactPositions where Tenant = @Tenant and Code = 'OTH')

			update Contacts
			set ContactPositionId = NULL
			where ContactPositionId = @PositionId and Tenant = @Tenant			
			
			delete from ContactPositions where Id = @PositionId and Tenant = @Tenant

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END

delete from ObjectFields where FieldName = 'Position' and ObjectTableId = (select Id from ObjectTables where Name = 'Contact')
delete from TextCodes where Code = 'Contact.F.Position'
delete from TextCodes where Code = 'Contact.PositionHelpText'