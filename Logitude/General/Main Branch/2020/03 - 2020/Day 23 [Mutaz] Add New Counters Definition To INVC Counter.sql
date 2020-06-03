-- Updated By Mutaz At 1/6/2020 To Set For Full Accounting Tenant Only

declare @Tenant as int
declare @NewEntityId as varchar(15)
declare @ARInvoiceCounterId as varchar(15)
declare @InvoiceObjectTableId as varchar(15)
set @InvoiceObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')

BEGIN
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
		WHILE @@FETCH_STATUS = 0
			BEGIN           
             if  exists (select * from Tenants where Id = @Tenant AND AccountingActivated = 1)
			  begin
				if not exists (select * from Counters where Tenant = @Tenant AND Code = 'INVC' AND ObjectTableId = @InvoiceObjectTableId and Id in (select CounterId from CounterDefinitions))
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'Counter'
					insert into Counters (Id, Tenant, Code, Name, ObjectTableId) values (@NewEntityId, @Tenant, 'INVC', 'A/R Invoice',@InvoiceObjectTableId)
				end

				set @ARInvoiceCounterId = (select Id from Counters where Tenant = @Tenant and ObjectTableId = @InvoiceObjectTableId and Code = 'INVC' and Id in (select CounterId from CounterDefinitions))

				if not exists (select * from CounterDefinitions where Tenant = @Tenant AND CounterId = @ARInvoiceCounterId AND Parameter1 = 'IT' )
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'CounterDefinition'
					insert into CounterDefinitions (Id, Tenant, Parameter1, StartNumber, CounterId, Prefix, UniquePerPrefix)
					values (@NewEntityId, @Tenant, 'IT', 1000, @ARInvoiceCounterId, NULL, 0)
				end

				if not exists (select * from CounterDefinitions where Tenant = @Tenant AND CounterId = @ARInvoiceCounterId AND Parameter1 = 'IC')
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'CounterDefinition'
					insert into CounterDefinitions (Id, Tenant, Parameter1, StartNumber, CounterId, Prefix, UniquePerPrefix)
					values (@NewEntityId, @Tenant, 'IC', 1000, @ARInvoiceCounterId, NULL, 0)
				end
              end
			FETCH NEXT FROM TenantsCursor INTO @Tenant
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END