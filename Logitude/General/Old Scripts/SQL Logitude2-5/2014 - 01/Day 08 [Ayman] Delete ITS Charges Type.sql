
declare @Tenant as int
declare @EntityId as varchar(15)
declare @LineNumber as int

BEGIN -- Payables
		DECLARE PayablesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM ShipmentPayables
		where ChargesTypeId IN (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'DIS')
		OPEN PayablesCursor FETCH NEXT FROM PayablesCursor INTO @EntityId, @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				update ShipmentPayables
				set ChargesTypeId = (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'SCH' and Tenant = @Tenant)
				where Id = @EntityId and Tenant = @Tenant

			FETCH NEXT FROM PayablesCursor INTO  @EntityId, @Tenant	
			END
		CLOSE PayablesCursor
		DEALLOCATE PayablesCursor
END

BEGIN -- Receivables
		DECLARE ReceivablesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM ShipmentReceivables
		where ChargesTypeId IN (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'DIS')
		OPEN ReceivablesCursor FETCH NEXT FROM ReceivablesCursor INTO @EntityId, @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				update ShipmentReceivables
				set ChargesTypeId = (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'SCH' and Tenant = @Tenant)
				where Id = @EntityId and Tenant = @Tenant

			FETCH NEXT FROM ReceivablesCursor INTO  @EntityId, @Tenant	
			END
		CLOSE ReceivablesCursor
		DEALLOCATE ReceivablesCursor
END

BEGIN -- ARInvoiceLines
		DECLARE ARInvoiceLinesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM ARInvoiceLines
		where ChargesTypeId IN (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'DIS')
		OPEN ARInvoiceLinesCursor FETCH NEXT FROM ARInvoiceLinesCursor INTO @EntityId, @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				update ARInvoiceLines
				set ChargesTypeId = (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'SCH' and Tenant = @Tenant)
				where Id = @EntityId and Tenant = @Tenant

			FETCH NEXT FROM ARInvoiceLinesCursor INTO  @EntityId, @Tenant	
			END
		CLOSE ARInvoiceLinesCursor
		DEALLOCATE ARInvoiceLinesCursor
END

BEGIN -- APInvoiceLines
		DECLARE APInvoiceLinesCursor CURSOR READ_ONLY
		FOR
		SELECT APInvoiceId, LineNumber, Tenant
		FROM APInvoiceLines
		where ChargesTypeId IN (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'DIS')
		OPEN APInvoiceLinesCursor FETCH NEXT FROM APInvoiceLinesCursor INTO @EntityId, @LineNumber, @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				update APInvoiceLines
				set ChargesTypeId = (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'SCH' and Tenant = @Tenant)
				where APInvoiceId = @EntityId and LineNumber = @LineNumber and Tenant = @Tenant

			FETCH NEXT FROM APInvoiceLinesCursor INTO  @EntityId, @LineNumber, @Tenant
			END
		CLOSE APInvoiceLinesCursor
		DEALLOCATE APInvoiceLinesCursor
END

BEGIN -- QuoteCharges
		DECLARE QuoteChargesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM QuoteCharges
		where ChargesTypeId IN (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'DIS')
		OPEN QuoteChargesCursor FETCH NEXT FROM QuoteChargesCursor INTO @EntityId, @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				update QuoteCharges
				set ChargesTypeId = (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'SCH' and Tenant = @Tenant)
				where Id = @EntityId and Tenant = @Tenant

			FETCH NEXT FROM QuoteChargesCursor INTO  @EntityId, @Tenant	
			END
		CLOSE QuoteChargesCursor
		DEALLOCATE QuoteChargesCursor
END

BEGIN -- TarrifCharges
		DECLARE TarrifChargesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM TarrifCharges
		where ChargesTypeId IN (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'DIS')
		OPEN TarrifChargesCursor FETCH NEXT FROM TarrifChargesCursor INTO @EntityId, @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				update TarrifCharges
				set ChargesTypeId = (select Id from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'SCH' and Tenant = @Tenant)
				where Id = @EntityId and Tenant = @Tenant

			FETCH NEXT FROM TarrifChargesCursor INTO  @EntityId, @Tenant	
			END
		CLOSE TarrifChargesCursor
		DEALLOCATE TarrifChargesCursor
END

delete from ChargesTypes where Code = 'ITS' and ChargesGroupCode = 'DIS'
go

