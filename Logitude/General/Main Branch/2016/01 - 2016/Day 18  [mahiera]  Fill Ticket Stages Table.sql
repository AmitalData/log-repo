declare @Tenant as int
declare @NewId as varchar(15)
declare @Id as varchAR(15)
declare @StageId as varchAR(15)
declare @StageCode as varchAR(2)

BEGIN
	DECLARE TicketsTablesCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TicketsTablesCursor FETCH NEXT FROM TicketsTablesCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	-- TicketStage
	BEGIN

		if not exists (select * from TicketStages where Code = 'OP' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketStage'
			insert into TicketStages(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'OP',
			'Open',
			0,
			'Open'
			)
		END

        if not exists (select * from TicketStages where Code = 'RE' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketStage'
			insert into TicketStages(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'RE',
			'Resolved',
			0,
			'Resolved'
			)
		END

		if not exists (select * from TicketStages where Code = 'CS' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketStage'
			insert into TicketStages(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'CS',
			'Closed',
			0,
			'Closed'
			)
		END

		if not exists (select * from TicketStages where Code = 'WC' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketStage'
			insert into TicketStages(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'WC',
			'Waiting On Customer',
			0,
			'Waiting On Customer'
			)
		END

		if not exists (select * from TicketStages where Code = 'WT' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TicketStage'
			insert into TicketStages(Id, Tenant, Code, Name, Inactive, SearchFields)
			values
			(
			@NewId,
			@Tenant,
			'WT',
			'Waiting On Third Party',
			0,
			'Waiting On Third Party'
			)
		END

	END

	FETCH NEXT FROM TicketsTablesCursor INTO  @Tenant
	END
	CLOSE TicketsTablesCursor
	DEALLOCATE TicketsTablesCursor

END

BEGIN 
	DECLARE TicketCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, StageId
	From Tickets
	OPEN TicketCursor FETCH NEXT FROM TicketCursor INTO @Id, @Tenant, @StageId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @StageCode = (select Code from TicketStages where Tenant = @Tenant AND Id = @StageId)

		if (@StageCode = 'CR' or @StageCode = 'IP' or @StageCode ='SO')
		begin
			set @StageId = (SELECT Id FROM TicketStages where Tenant = @Tenant and Code = 'OP')
			update Tickets set StageId = @StageId where Id = @Id
		end

	FETCH NEXT FROM TicketCursor INTO @Id, @Tenant, @StageId
	END

	CLOSE TicketCursor
	DEALLOCATE TicketCursor
END

delete from TicketStages where code = 'CR' or code ='IP' or code ='SO'