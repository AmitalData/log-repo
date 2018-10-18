

--select count(*), Tenant, ARInvoiceId, EntityId
--from ARInvoiceEntities
--group by Tenant, ARInvoiceId, EntityId
--HAVING (COUNT(*) > 1)

--select count(*), Tenant, APInvoiceId, EntityId
--from APInvoiceEntities
--group by Tenant, APInvoiceId, EntityId
--HAVING (COUNT(*) > 1)

DELETE FROM ARInvoiceEntities WHERE Id NOT IN (SELECT MIN(Id) FROM ARInvoiceEntities GROUP BY Tenant, ARInvoiceId, EntityId)
go

DELETE FROM APInvoiceEntities WHERE Id NOT IN (SELECT MIN(Id) FROM APInvoiceEntities GROUP BY Tenant, APInvoiceId, EntityId)
go

declare @Tenant as int
declare @EntityId as varchar(15)
declare @InvoiceEntityId as varchar(15)


-- (AR) InvoiceEntities
	DECLARE ARInvoiceEntitiesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id, Tenant, EntityId
	FROM ARInvoiceEntities
	where EntityReference is null
	OPEN ARInvoiceEntitiesCursor FETCH NEXT FROM ARInvoiceEntitiesCursor INTO @InvoiceEntityId, @Tenant, @EntityId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		update ARInvoiceEntities
		set EntityReference = (select ShipmentNumber from Shipments where Id = @EntityId AND Tenant = @Tenant)
		where Tenant = Tenant AND Id = @InvoiceEntityId

	FETCH NEXT FROM ARInvoiceEntitiesCursor INTO @InvoiceEntityId, @Tenant, @EntityId
	END
	CLOSE ARInvoiceEntitiesCursor
	DEALLOCATE ARInvoiceEntitiesCursor	


-- (AP) InvoiceEntities
	DECLARE APInvoiceEntitiesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id, Tenant, EntityId
	FROM APInvoiceEntities
	where EntityReference is null
	OPEN APInvoiceEntitiesCursor FETCH NEXT FROM APInvoiceEntitiesCursor INTO @InvoiceEntityId, @Tenant, @EntityId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		update APInvoiceEntities
		set EntityReference = (select ShipmentNumber from Shipments where Id = @EntityId AND Tenant = @Tenant)
		where Tenant = Tenant AND Id = @InvoiceEntityId

	FETCH NEXT FROM APInvoiceEntitiesCursor INTO @InvoiceEntityId, @Tenant, @EntityId
	END
	CLOSE APInvoiceEntitiesCursor
	DEALLOCATE APInvoiceEntitiesCursor	