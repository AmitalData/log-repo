
declare @Tenant as int
declare @NewId as varchar(15)

BEGIN
	DECLARE TicketsTablesCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TicketsTablesCursor FETCH NEXT FROM TicketsTablesCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	-- TicketType
	BEGIN
		if not exists (select * from TicketTypes where Code = 'QU' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketType'
			insert into TicketTypes(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'QU',
			'Question',
			0,
			'Question'
			)
		END

		if not exists (select * from TicketTypes where Code = 'PR' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketType'
			insert into TicketTypes(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'PR',
			'Problem',
			0,
			'Problem'
			)
		END
	END

	-- TicketSeverity
	BEGIN
		if not exists (select * from TicketSeverities where Code = 'UR' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketSeverity'
			insert into TicketSeverities(Id, Tenant, Code, Name, Severity,Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'UR',
			'Urgent',
			2,
			0,
			'Urgent'
			)
		END

		if not exists (select * from TicketSeverities where Code = 'HI' and Tenant = @Tenant)
			BEGIN
				EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketSeverity'
				insert into TicketSeverities(Id, Tenant, Code, Name, Severity,Inactive, SearchFields)
				values
				(
				@NewId,
				@Tenant,
				'HI',
				'High',
				3,
				0,
				'High'
				)
		END

		if not exists (select * from TicketSeverities where Code = 'CT' and Tenant = @Tenant)
			BEGIN
				EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketSeverity'
				insert into TicketSeverities(Id, Tenant, Code, Name, Severity,Inactive, SearchFields)
				values
				(
				@NewId,
				@Tenant,
				'CT',
				'Critical',
				1,
				0,
				'Critical'
				)
		END

		if not exists (select * from TicketSeverities where Code = 'MD' and Tenant = @Tenant)
			BEGIN
				EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketSeverity'
				insert into TicketSeverities(Id, Tenant, Code, Name, Severity,Inactive, SearchFields)
				values
				(
				@NewId,
				@Tenant,
				'MD',
				'Medium',
				4,
				0,
				'Medium'
				)
			END
	
		if not exists (select * from TicketSeverities where Code = 'LW' and Tenant = @Tenant)
			BEGIN
				EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketSeverity'
				insert into TicketSeverities(Id, Tenant, Code, Name, Severity,Inactive, SearchFields)
				values
				(
				@NewId,
				@Tenant,
				'LW',
				'Low',
				5,
				0,
				'Low'
				)
			END
	END

	-- TicketStage
	BEGIN
		if not exists (select * from TicketStages where Code = 'CR' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketStage'
			insert into TicketStages(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'CR',
			'Created',
			0,
			'Created'
			)
		END

        if not exists (select * from TicketStages where Code = 'IP' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketStage'
			insert into TicketStages(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'IP',
			'In Progress',
			0,
			'In Progress'
			)
		END

		if not exists (select * from TicketStages where Code = 'SO' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketStage'
			insert into TicketStages(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'SO',
			'Solved',
			0,
			'Solved'
			)
		END

	END

	-- TicketClassification
	BEGIN
		if not exists (select * from TicketClassifications where Name = 'General' and ParentId = null and Tenant = @Tenant)
		BEGIN
			insert into TicketClassifications(Id, Tenant, Name, Inactive, SearchFields)
			values
			(
			@Tenant,
			@Tenant,
			'General',
			0,
			'General'
			)
		END

	END

	FETCH NEXT FROM TicketsTablesCursor INTO  @Tenant
	END
	CLOSE TicketsTablesCursor
	DEALLOCATE TicketsTablesCursor

END