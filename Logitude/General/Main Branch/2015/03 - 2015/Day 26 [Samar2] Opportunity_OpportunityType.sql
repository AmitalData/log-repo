declare @Tenant as int
declare @TypeId as varchar(15)
declare @Tenant0Id as varchar(15)
declare @TypeCode as varchar(1)
BEGIN
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if (@Tenant = 0)
			BEGIN

				set @TypeCode  = 'E'
				set @Tenant0Id = (select Id from OpportunityTypes where Tenant = @Tenant and Code = @TypeCode)
				if(@Tenant0Id is null or @Tenant0Id = '')
				begin
				EXECUTE usp_GetNextTableIdValue @TypeId OUTPUT,'OpportunityType'
				update OpportunityTypes set Id = @TypeId where Tenant = @Tenant and Code = @TypeCode
				end

				set @TypeCode  = 'N'
				set @Tenant0Id = (select Id from OpportunityTypes where Tenant = @Tenant and Code = @TypeCode)
				if(@Tenant0Id is null or @Tenant0Id = '')
				begin
				EXECUTE usp_GetNextTableIdValue @TypeId OUTPUT,'OpportunityType'
				update OpportunityTypes set Id = @TypeId where Tenant = @Tenant and Code = @TypeCode
				end

				set @TypeCode  = 'R'
				set @Tenant0Id = (select Id from OpportunityTypes where Tenant = @Tenant and Code = @TypeCode)
				if(@Tenant0Id is null or @Tenant0Id = '')
				begin
				EXECUTE usp_GetNextTableIdValue @TypeId OUTPUT,'OpportunityType'
				update OpportunityTypes set Id = @TypeId where Tenant = @Tenant and Code = @TypeCode
				end

				set @TypeCode  = 'T'
				set @Tenant0Id = (select Id from OpportunityTypes where Tenant = @Tenant and Code = @TypeCode)
				if(@Tenant0Id is null or @Tenant0Id = '')
				begin
				EXECUTE usp_GetNextTableIdValue @TypeId OUTPUT,'OpportunityType'
				update OpportunityTypes set Id = @TypeId where Tenant = @Tenant and Code = @TypeCode
				end
			END

			ELSE
			BEGIN
				if  (select Id from OpportunityTypes where Tenant = @Tenant and Code = 'E') is null
				begin			
						EXECUTE usp_GetNextTableIdValue @TypeId OUTPUT,'OpportunityType'
						insert into OpportunityTypes(Id, Tenant, Code, Name, SearchFields,InActive)
						values(@TypeId, @Tenant, 'E', 'Expansion', 'E,Expansion', 0)
				end

				if (select Id from OpportunityTypes where Tenant = @Tenant and Code = 'N') is null
				begin
						EXECUTE usp_GetNextTableIdValue @TypeId OUTPUT,'OpportunityType'
						insert into OpportunityTypes(Id, Tenant, Code, Name, SearchFields,InActive)
						values(@TypeId, @Tenant, 'N', 'New Business', 'N,New Business', 0)
				end

				if (select Id from OpportunityTypes where Tenant = @Tenant and Code = 'R') is null
				begin
						EXECUTE usp_GetNextTableIdValue @TypeId OUTPUT,'OpportunityType'
						insert into OpportunityTypes(Id, Tenant, Code, Name, SearchFields,InActive)
						values(@TypeId, @Tenant, 'R', 'Bid', 'R,Bid', 0)
				end

				if (select Id from OpportunityTypes where Tenant = @Tenant and Code = 'T') is null
				begin
						EXECUTE usp_GetNextTableIdValue @TypeId OUTPUT,'OpportunityType'
						insert into OpportunityTypes(Id, Tenant, Code, Name, SearchFields,InActive)
						values(@TypeId, @Tenant, 'T', 'Routing Order', 'T,Routing Order', 0)
				end		
			END
				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END

