
declare @TypeCode as varchar(1)
declare @TypeId as varchar(15)
declare @Tenant as int

BEGIN
		DECLARE OpportunitiesCursor CURSOR READ_ONLY
		FOR
		SELECT OpportunityTypeCode, Tenant
		FROM Opportunities
		OPEN OpportunitiesCursor FETCH NEXT FROM OpportunitiesCursor INTO @TypeCode, @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @TypeId = (select Id from OpportunityTypes where Code = @TypeCode and Tenant = @Tenant)

			update Opportunities
			set OpportunityTypeId = @TypeId
			where OpportunityTypeCode = @TypeCode and Tenant = @Tenant			

				FETCH NEXT FROM OpportunitiesCursor INTO @TypeCode, @Tenant	
			END
		CLOSE OpportunitiesCursor
		DEALLOCATE OpportunitiesCursor
END
go
