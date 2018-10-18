-- delete from Activities where TicketId in (select Id from Tickets where SeverityId = (select Id from TicketSeverities where code='CT' and Tenant=1))

-- delete  from Tickets where SeverityId = (select Id from TicketSeverities where code='CT' and Tenant=1)

-- delete from TicketSeverities where code ='CT'

-- loop tickts
-- sevitty id
-- severtity code
-- >> critical ? replace it with the other

declare @Tenant integer
declare @Id as varchAR(15)
declare @SeverityId as varchAR(15)

BEGIN 
	DECLARE TicketCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, SeverityId
	From Tickets
	OPEN TicketCursor FETCH NEXT FROM TicketCursor INTO @Id, @Tenant, @SeverityId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		if (select Code from TicketSeverities where Tenant = @Tenant AND Id = @SeverityId) = 'CT'
		begin
			set @SeverityId = (SELECT Id FROM TicketSeverities where Tenant = @Tenant and Code = 'UR')
			update Tickets set SeverityId = @SeverityId where Id = @Id
		end

	FETCH NEXT FROM TicketCursor INTO @Id, @Tenant, @SeverityId
	END

	CLOSE TicketCursor
	DEALLOCATE TicketCursor
END

delete from TicketSeverities where code = 'CT'