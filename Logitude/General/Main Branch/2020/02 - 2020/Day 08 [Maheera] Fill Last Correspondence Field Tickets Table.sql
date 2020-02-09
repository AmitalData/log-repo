declare @Tenant as int
declare @EntityId as varchar(15)
declare @LastCorrespondenceLine as nvarchar(4000)
declare @TicketDescription as nvarchar(4000)

BEGIN
		DECLARE TicketsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,TicketDescription, Tenant
		FROM Tickets
		OPEN TicketsCursor FETCH NEXT FROM TicketsCursor INTO @EntityId, @TicketDescription, @Tenant
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @LastCorrespondenceLine = (SELECT top(1) Description FROM Correspondences where Tenant = @Tenant and EntityId = @EntityId ORDER BY CreateDate desc)
		if(@LastCorrespondenceLine is null)
		begin
			set @LastCorrespondenceLine = @TicketDescription;
		end

		update Tickets set LastCorrespondence = @LastCorrespondenceLine where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM TicketsCursor INTO @EntityId, @TicketDescription, @Tenant
		END				
		CLOSE TicketsCursor
		DEALLOCATE TicketsCursor
END