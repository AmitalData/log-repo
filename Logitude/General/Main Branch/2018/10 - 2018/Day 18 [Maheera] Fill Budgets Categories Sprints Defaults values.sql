
declare @Tenant as int
declare @NewId as varchar(15)
declare @ContactId as varchar(15)

BEGIN
	DECLARE TimeManagementsTablesCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TimeManagementsTablesCursor FETCH NEXT FROM TimeManagementsTablesCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	   BEGIN
		if not exists (select * from TMBudgets where Name = 'General' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TMBudget'
			insert into TMBudgets(Id, Tenant, Name, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'General',
			'General'
			)
			update TMProjects set BudgetId = @NewId where Tenant = @Tenant
		END
		END
	   BEGIN
		if not exists (select * from TMProjectCategories where Name = 'General' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TMProjectCategory'
			insert into TMProjectCategories(Id, Tenant, Name,Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'General',
			'0',
			'General'
			)

			update TMProjects set CategoryId = @NewId where Tenant = @Tenant
		END
		END
       BEGIN
		if not exists (select * from Sprints where Name = 'General' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'Sprint'
			set @ContactId = (select top 1 Id from Contacts where Tenant = @Tenant AND  UserType = 'S' And Email like '%system%')
			insert into Sprints(Id, Tenant, Name,FromDate, ToDate,CreatedByUserId, UpdatedByUserId, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'General',
			GETDATE(),
			GETDATE(),
			@ContactId,
		    @ContactId,
			'General'
			)

			update TMProjects set CategoryId = @NewId where Tenant = @Tenant
		END
		END

	FETCH NEXT FROM TimeManagementsTablesCursor INTO  @Tenant
	END
	CLOSE TimeManagementsTablesCursor
	DEALLOCATE TimeManagementsTablesCursor
END