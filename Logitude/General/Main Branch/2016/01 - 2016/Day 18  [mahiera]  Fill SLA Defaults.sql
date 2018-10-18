declare @Tenant as int
declare @NewId as varchar(15)
declare @NewLineId as varchar(15)
declare @ContactId as varchar(15)
declare @SeverityId as varchar(15)
declare @BusinessHoursId as varchar (15)

BEGIN
	DECLARE SLAHeadersTablesCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN SLAHeadersTablesCursor FETCH NEXT FROM SLAHeadersTablesCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	-- SLA Headers
	BEGIN

		if not exists (select * from SLAHeaders where Name = 'SLA' and Tenant = @Tenant)
		BEGIN
			-- SLA 
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'SLAHeader'
			set @ContactId = (select top 1 Id from Contacts where Tenant = @Tenant AND UserType = 'S' And Email like '%system%')
			insert into SLAHeaders(Id, Tenant,CreateDate, CreatedByUserId,UpdateDate, UpdatedByUserId,Name)
			values
			(
			@NewId,
			@Tenant,
			GETDATE(),
			@ContactId,
			GETDATE(),
			@ContactId,
			'SLA'
			)

			-- SLALines
			BEGIN

			set @BusinessHoursId = (select Id from BusinessHours where Tenant = @Tenant AND Name = 'Business Hours')

			-- Low 
			EXECUTE usp_GetNextTableIdValue @NewLineId OUTPUT,'SLALine'
			set @SeverityId = (select Id from TicketSeverities where Tenant = @Tenant AND Code = 'LW')
			insert into SLALines(Id, Tenant,SLAHeaderId,SeverityId,BusinessHoursId,FirstResponseTimeUnit,FirstResponseTimeInMinute,
				ResolveWithinTimeUnit,ResolveWithinTimeInMinute,FirstResponseEscalate,ResolveWithinEscalate,FirstResponseTime,ResolveWithinTime)
				values
				(
				@NewLineId,
				@Tenant,
				@NewId,
				@SeverityId,
				@BusinessHoursId,
				'OO',
				'360',
				'OO',
				'720',
				'1',
				'1',
				'6',
				'12'
				)
			
			-- Medium 
			EXECUTE usp_GetNextTableIdValue @NewLineId OUTPUT,'SLALine'
			set @SeverityId = (select Id from TicketSeverities where Tenant = @Tenant AND Code = 'MD')
			insert into SLALines(Id, Tenant,SLAHeaderId,SeverityId,BusinessHoursId,FirstResponseTimeUnit,FirstResponseTimeInMinute,
				ResolveWithinTimeUnit,ResolveWithinTimeInMinute,FirstResponseEscalate,ResolveWithinEscalate,FirstResponseTime,ResolveWithinTime)
				values
				(
				@NewLineId,
				@Tenant,
				@NewId,
				@SeverityId,
				@BusinessHoursId,
				'OO',
				'180',
				'OO',
				'360',
				'1',
				'1',
				'3',
				'6'
				)

			-- High 
			EXECUTE usp_GetNextTableIdValue @NewLineId OUTPUT,'SLALine'
			set @SeverityId = (select Id from TicketSeverities where Tenant = @Tenant AND Code = 'HI')
			insert into SLALines(Id, Tenant,SLAHeaderId,SeverityId,BusinessHoursId,FirstResponseTimeUnit,FirstResponseTimeInMinute,
				ResolveWithinTimeUnit,ResolveWithinTimeInMinute,FirstResponseEscalate,ResolveWithinEscalate,FirstResponseTime,ResolveWithinTime)
				values
				(
				@NewLineId,
				@Tenant,
				@NewId,
				@SeverityId,
				@BusinessHoursId,
				'OO',
				'120',
				'OO',
				'240',
				'1',
				'1',
				'2',
				'4'
				)

			-- Urgent 
			EXECUTE usp_GetNextTableIdValue @NewLineId OUTPUT,'SLALine'
			set @SeverityId = (select Id from TicketSeverities where Tenant = @Tenant AND Code = 'UR')
			insert into SLALines(Id, Tenant,SLAHeaderId,SeverityId,BusinessHoursId,FirstResponseTimeUnit,FirstResponseTimeInMinute,
				ResolveWithinTimeUnit,ResolveWithinTimeInMinute,FirstResponseEscalate,ResolveWithinEscalate,FirstResponseTime,ResolveWithinTime)
				values
				(
				@NewLineId,
				@Tenant,
				@NewId,
				@SeverityId,
				@BusinessHoursId,
				'OO',
				'60',
				'OO',
				'120',
				'1',
				'1',
				'1',
				'2'
				)

			END

		End

	END

	FETCH NEXT FROM SLAHeadersTablesCursor INTO  @Tenant
	END
	CLOSE SLAHeadersTablesCursor
	DEALLOCATE SLAHeadersTablesCursor

END