update ObjectTables set AllowedInQueues = '0'
update ObjectTables set AllowedInQueues = '1' where name in ('shipment','quote')

declare @Tenant as int
declare @Id as varchar(15)
declare @EntityId as varchar(60)
declare @ShipmentTableId as varchar(60)
declare @QuoteTableId as varchar(60)

BEGIN
	DECLARE TicketsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	FROM Tickets
	OPEN TicketsCursor FETCH NEXT FROM TicketsCursor INTO @Id , @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	        set @ShipmentTableId = (select Id from ObjectTables where name = 'shipment')
			set @QuoteTableId = (select Id from ObjectTables where name = 'quote')
		    set @EntityId = (select ShipmentId from Tickets where Tenant = @Tenant and Id = @Id)
			if (@EntityId is not null)
				update Tickets set EntityType = @ShipmentTableId where  Tenant = @Tenant and Id = @Id

			set @EntityId = (select QuoteId from Tickets where Tenant = @Tenant and Id = @Id)
			if (@EntityId is not null)
				update Tickets set EntityType = @QuoteTableId where  Tenant = @Tenant and Id = @Id

	FETCH NEXT FROM TicketsCursor INTO  @Id , @Tenant
	END
	CLOSE TicketsCursor
	DEALLOCATE TicketsCursor
END