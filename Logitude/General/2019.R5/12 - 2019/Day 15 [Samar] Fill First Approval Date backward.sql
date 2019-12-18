

declare @Tenant as int
declare @EntityId as varchar(15)
declare @EventTypeId as varchar(15)
declare @ObjectTableId as varchar(15)
declare @ObjectTableId2 as varchar(15)
declare @FirstEventDate as datetime
declare @ShipmentId as varchar(15)
declare @IsConsolidationInvoice as bit

set @ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')

DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN

	set @EventTypeId = (select Id from EventTypes where Code = 'INAP' AND Tenant = @Tenant AND ObjectTableId = @ObjectTableId)
	if (@EventTypeId is not null)
	begin
		if exists (select * from TraceEvents where Tenant = @Tenant AND EventTypeId = @EventTypeId AND ObjectTableId = @ObjectTableId)
		begin

			DECLARE TraceEventsCursor CURSOR READ_ONLY
			FOR
			SELECT EntityId
			FROM TraceEvents where Tenant = @Tenant AND EventTypeId = @EventTypeId AND ObjectTableId = @ObjectTableId
			Group by EntityId
			OPEN TraceEventsCursor FETCH NEXT FROM TraceEventsCursor INTO @EntityId
			WHILE @@FETCH_STATUS = 0
			BEGIN

				set @FirstEventDate = (select top 1 LogDateTime from TraceEvents 
									   where Tenant = @Tenant
								       AND EventTypeId = @EventTypeId
								       AND ObjectTableId = @ObjectTableId
								       AND EntityId = @EntityId
								       order by LogDateTime)

				set @IsConsolidationInvoice = (select IsConsolidationInvoice from ARInvoices where Id = @EntityId and Tenant = @Tenant)

				if(@IsConsolidationInvoice = 1)
				begin
					--print @IsConsolidationInvoice
					--print '-----------------'

					DECLARE ConstituentsCursor CURSOR READ_ONLY
					FOR
					SELECT MainEntityId
					FROM ARInvoices
					WHERE ConsolidationInvoiceId = @EntityId and IsConstituentInvoice = 1
					OPEN ConstituentsCursor FETCH NEXT FROM ConstituentsCursor INTO @ShipmentId
					WHILE @@FETCH_STATUS = 0
					BEGIN
						if(@ShipmentId is not null)
						begin						
							update Shipments set FirstARInvoiceApprovalDate = @FirstEventDate where Id = @ShipmentId
						end
					FETCH NEXT FROM ConstituentsCursor INTO @ShipmentId
					END
					CLOSE ConstituentsCursor
					DEALLOCATE ConstituentsCursor

				end
				
				else 
				begin
					set @ShipmentId = (select MainEntityId from ARInvoices where Id = @EntityId and Tenant = @Tenant)
					if(@ShipmentId is not null)
					begin
						--print @ShipmentId
						--print @FirstEventDate
						--print '-----------------'
						update Shipments set FirstARInvoiceApprovalDate = @FirstEventDate where Id = @ShipmentId
					end		

				end	

			FETCH NEXT FROM TraceEventsCursor INTO @EntityId
			END
			CLOSE TraceEventsCursor
			DEALLOCATE TraceEventsCursor

		end
	end

FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor