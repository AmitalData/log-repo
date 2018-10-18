declare @Tenant as int
declare @DefaultId as varchAR(15)
declare @TicketId as varchAR(15)

BEGIN 
              DECLARE TicketsCursor CURSOR READ_ONLY
              FOR
              SELECT Id, Tenant
              FROM Tickets
              OPEN TicketsCursor FETCH NEXT FROM TicketsCursor INTO @TicketId, @Tenant       
              WHILE @@FETCH_STATUS = 0
                     BEGIN
					 set @DefaultId = (select top(1) DefaultSLAId FROM Tenants where Id = @Tenant)
		             update Tickets set SLAId = @DefaultId where Id = @TicketId and Tenant = @Tenant
                     FETCH NEXT FROM TicketsCursor INTO @TicketId, @Tenant       
                     END
              CLOSE TicketsCursor
              DEALLOCATE TicketsCursor
END