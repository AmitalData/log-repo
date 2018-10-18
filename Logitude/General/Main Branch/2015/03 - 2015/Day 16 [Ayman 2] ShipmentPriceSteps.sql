
declare @Tenant as int
declare @QuoteId as varchar(15)
declare @ShipmentId as varchar(15)
declare @PayableId as varchar(15)
declare @ReceivableId as varchar(15)
declare @ChargesTypeId as varchar(15)
declare @ChargesGroupCode as varchar(10)
declare @IsQuoteBySteps as bit

declare @QuoteChargeId as varchar(15)
declare @IsChargeBySteps as bit

BEGIN
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, QuoteId
	FROM Shipments
	where QuoteId is not null
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @QuoteId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @IsQuoteBySteps = (select IsFreightBySteps from Quotes where Id = @QuoteId AND Tenant = @Tenant)
		if (@IsQuoteBySteps = 1)
		BEGIN

		-- Loop Payables
		BEGIN
			DECLARE PayablesCursor CURSOR READ_ONLY
			FOR
			SELECT Id, ChargesTypeId
			FROM ShipmentPayables
			where ShipmentId = @ShipmentId AND Tenant = @Tenant AND IsFromQuote = 1 AND QuoteChargeId is null
			OPEN PayablesCursor FETCH NEXT FROM PayablesCursor INTO @PayableId, @ChargesTypeId
			WHILE @@FETCH_STATUS = 0
			BEGIN
				
				set @ChargesGroupCode = (select ChargesGroupCode from ChargesTypes where Id = @ChargesTypeId AND Tenant = @Tenant)
				if (@ChargesGroupCode = 'FRT')
				BEGIN
					set @QuoteChargeId = null
					set @IsChargeBySteps = 0					

					if exists (select * from QuoteCharges where QuoteId = @QuoteId AND ChargesTypeId = @ChargesTypeId AND Tenant = @Tenant)
					BEGIN
						set @QuoteChargeId = (select Min(Id) from QuoteCharges where QuoteId = @QuoteId AND ChargesTypeId = @ChargesTypeId AND Tenant = @Tenant)
						set @IsChargeBySteps = (select IsChargeBySteps from QuoteCharges where QuoteId = @QuoteId AND ChargesTypeId = @ChargesTypeId AND Tenant = @Tenant AND Id = @QuoteChargeId)
					END

					update ShipmentPayables
					set QuoteChargeId = @QuoteChargeId, IsChargeBySteps = @IsChargeBySteps
					where Id = @PayableId AND Tenant = @Tenant
				END

			FETCH NEXT FROM PayablesCursor INTO @PayableId, @ChargesTypeId
			END
			CLOSE PayablesCursor
			DEALLOCATE PayablesCursor
		END

		--Loop Receivables
		BEGIN
			DECLARE ReceivablesCursor CURSOR READ_ONLY
			FOR
			SELECT Id, ChargesTypeId
			FROM ShipmentReceivables
			where ShipmentId = @ShipmentId AND Tenant = @Tenant AND IsFromQuote = 1 AND QuoteChargeId is null
			OPEN ReceivablesCursor FETCH NEXT FROM ReceivablesCursor INTO @ReceivableId, @ChargesTypeId
			WHILE @@FETCH_STATUS = 0
			BEGIN

				set @ChargesGroupCode = (select ChargesGroupCode from ChargesTypes where Id = @ChargesTypeId AND Tenant = @Tenant)
				if (@ChargesGroupCode = 'FRT')
				BEGIN
					set @QuoteChargeId = null
					set @IsChargeBySteps = 0

					if exists (select * from QuoteCharges where QuoteId = @QuoteId AND ChargesTypeId = @ChargesTypeId AND Tenant = @Tenant)
					BEGIN
						set @QuoteChargeId = (select Min(Id) from QuoteCharges where QuoteId = @QuoteId AND ChargesTypeId = @ChargesTypeId AND Tenant = @Tenant)
						set @IsChargeBySteps = (select IsChargeBySteps from QuoteCharges where QuoteId = @QuoteId AND ChargesTypeId = @ChargesTypeId AND Tenant = @Tenant AND Id = @QuoteChargeId)
					END

					update ShipmentReceivables
					set QuoteChargeId = @QuoteChargeId, IsChargeBySteps = @IsChargeBySteps
					where Id = @ReceivableId AND Tenant = @Tenant
				END

			FETCH NEXT FROM ReceivablesCursor INTO @ReceivableId, @ChargesTypeId
			END
			CLOSE ReceivablesCursor
			DEALLOCATE ReceivablesCursor
		END

		END
	FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @QuoteId
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END
