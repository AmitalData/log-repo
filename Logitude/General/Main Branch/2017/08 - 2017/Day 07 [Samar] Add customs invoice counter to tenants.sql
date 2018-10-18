
declare @Tenant as int
declare @NewEntityId as varchar(15)
declare @CustomsCounterId as varchar(15)

declare @InvoiceObjectTableId as varchar(15)
set @InvoiceObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')

declare @Prefix as varchar(10)
declare @UniquePerPrefix as bit


BEGIN
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
		WHILE @@FETCH_STATUS = 0
			BEGIN

				if not exists (select * from Counters where Tenant = @Tenant AND Code = 'CUST' AND ObjectTableId = @InvoiceObjectTableId)
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'Counter'
					insert into Counters (Id, Tenant, Code, Name, ObjectTableId) values (@NewEntityId, @Tenant, 'CUST', 'Customs Invoice',@InvoiceObjectTableId)
				end

				set @CustomsCounterId = (select Id from Counters where Tenant = @Tenant and ObjectTableId = @InvoiceObjectTableId and Code = 'CUST')

				if not exists (select * from CounterDefinitions where Tenant = @Tenant AND CounterId = @CustomsCounterId AND Parameter1 = 'CA')
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'CounterDefinition'
					insert into CounterDefinitions (Id, Tenant, Parameter1, StartNumber, CounterId, Prefix, UniquePerPrefix)
					values (@NewEntityId, @Tenant, 'CA', 1000, @CustomsCounterId, 'CA', 0)
				end

			FETCH NEXT FROM TenantsCursor INTO @Tenant
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END