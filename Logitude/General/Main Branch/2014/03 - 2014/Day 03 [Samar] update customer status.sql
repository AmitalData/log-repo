
update Customers set CustomerStatusCode = 'ACT'
go


declare @CardId as varchar(15)
declare @Tenant as int

BEGIN -- CardsCursor
		DECLARE CardsCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM Cards
		WHERE PartnerTypeId = 'CS' and InActive = 1
		OPEN CardsCursor FETCH NEXT FROM CardsCursor INTO @CardId, @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			update Customers 
			set CustomerStatusCode = 'INA'
			where Id = @CardId and Tenant = @Tenant

			FETCH NEXT FROM CardsCursor INTO @CardId, @Tenant	
			END
		CLOSE CardsCursor
		DEALLOCATE CardsCursor
END