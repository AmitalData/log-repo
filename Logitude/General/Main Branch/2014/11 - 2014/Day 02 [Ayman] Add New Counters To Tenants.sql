
declare @Tenant as int
declare @NewEntityId as varchar(15)
declare @ARInvoiceCounterId as varchar(15)
declare @ConstituentCounterId as varchar(15)

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

				if not exists (select * from Counters where Tenant = @Tenant AND Code = 'CNST' AND ObjectTableId = @InvoiceObjectTableId)
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'Counter'
					insert into Counters (Id, Tenant, Code, Name, ObjectTableId) values (@NewEntityId, @Tenant, 'CNST', 'Constituent Invoice',@InvoiceObjectTableId)
				end

				set @ConstituentCounterId = (select Id from Counters where Tenant = @Tenant and ObjectTableId = @InvoiceObjectTableId and Code = 'CNST')

				if not exists (select * from CounterDefinitions where Tenant = @Tenant AND CounterId = @ConstituentCounterId AND Parameter1 = 'CNS')
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'CounterDefinition'
					insert into CounterDefinitions (Id, Tenant, Parameter1, StartNumber, CounterId, Prefix, UniquePerPrefix)
					values (@NewEntityId, @Tenant, 'CNS', 1000, @ConstituentCounterId, 'CNS', 0)
				end




				if not exists (select * from Counters where Tenant = @Tenant AND Code = 'INVC' AND ObjectTableId = @InvoiceObjectTableId)
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'Counter'
					insert into Counters (Id, Tenant, Code, Name, ObjectTableId) values (@NewEntityId, @Tenant, 'INVC', 'ARInvoice',@InvoiceObjectTableId)
				end

				set @ARInvoiceCounterId = (select Id from Counters where Tenant = @Tenant and ObjectTableId = @InvoiceObjectTableId and Code = 'INVC')

				if not exists (select * from CounterDefinitions where Tenant = @Tenant AND CounterId = @ARInvoiceCounterId AND Parameter1 = 'CON')
				begin

					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'CounterDefinition'

					set @Prefix = null
					set @UniquePerPrefix = 0

					if exists (select * from CounterDefinitions where Tenant = @Tenant and CounterId = @ARInvoiceCounterId and Parameter1 = 'IN')
					begin
						select 
						@Prefix = Prefix, 
						@UniquePerPrefix = UniquePerPrefix
						from CounterDefinitions 
						where Tenant = @Tenant and CounterId = @ARInvoiceCounterId and Parameter1 = 'IN'

						if (@Prefix is not null)
						set @Prefix = 'CON'
					end

					insert into CounterDefinitions (Id, Tenant, Parameter1, StartNumber, CounterId, Prefix, UniquePerPrefix)
					values (@NewEntityId, @Tenant, 'CON', 1000, @ARInvoiceCounterId, @Prefix, @UniquePerPrefix)
				end


			FETCH NEXT FROM TenantsCursor INTO @Tenant
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END


select * from CounterDefinitions
where (Parameter1 = 'IN' OR Parameter1 = 'CD' OR Parameter1 = 'MN' OR Parameter1 = 'CON')
and CounterId in (select Id from Counters where ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice'))
and Tenant = 1
go

