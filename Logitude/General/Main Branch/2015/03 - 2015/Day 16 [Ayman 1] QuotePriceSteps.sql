
declare @Tenant as int
declare @QuoteId as varchar(15)
declare @QuoteTypeCode as varchar(1)
declare @TransportModeId as varchar(1)
declare @ShipmentTypeId as varchar(5)
declare @IsLCLQuote as bit
declare @IsFreightBySteps as bit

declare @QuoteChargeId as varchar(15)
declare @ChargesTypeId as varchar(15)
declare @ChargesGroupCode as varchar(10)

declare @NewId as varchar(15)
declare @Step as float
declare @SaleUnitPrice as float
declare @CostUnitPrice as float
declare @MarkupValue as float

BEGIN
	DECLARE QuotesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, QuoteTypeCode, TransportModeId, ShipmentTypeId, IsFreightBySteps
	FROM Quotes
	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @QuoteTypeCode, @TransportModeId, @ShipmentTypeId, @IsFreightBySteps
	WHILE @@FETCH_STATUS = 0
	BEGIN

	-- LCL | FCL
	BEGIN
		if (@TransportModeId = 'A')
		set @IsLCLQuote = 1

		else if (@TransportModeId = 'O' AND @ShipmentTypeId = 'LCLD')
		set @IsLCLQuote = 1

		else if (@TransportModeId = 'I' AND @ShipmentTypeId = 'LTL')
		set @IsLCLQuote = 1

		else
		set @IsLCLQuote = 0
	END

	if (@IsLCLQuote = 0 OR @QuoteTypeCode = 'A')
	begin
		set @IsFreightBySteps = 0		
		update Quotes set IsFreightBySteps = 0 where Id = @QuoteId AND Tenant = @Tenant
		update QuoteCharges set IsChargeBySteps = 0 where QuoteId = @QuoteId AND Tenant = @Tenant
	end

	BEGIN
		if (@IsFreightBySteps = 1)
		BEGIN
		
			DECLARE ChargesCursor CURSOR READ_ONLY
			FOR
			SELECT Id, ChargesTypeId
			FROM QuoteCharges
			where QuoteId = @QuoteId AND Tenant = @Tenant
			OPEN ChargesCursor FETCH NEXT FROM ChargesCursor INTO @QuoteChargeId, @ChargesTypeId
			WHILE @@FETCH_STATUS = 0
			BEGIN

				set @ChargesGroupCode = (select ChargesGroupCode from ChargesTypes where Id = @ChargesTypeId AND Tenant = @Tenant)
				if (@ChargesGroupCode = 'FRT')
				BEGIN
					
					update QuoteCharges set IsChargeBySteps = 1 where QuoteId = @QuoteId AND Tenant = @Tenant AND Id = @QuoteChargeId

					if not exists (select * from QuotePriceSteps where QuoteId = @QuoteId AND Tenant = @Tenant AND QuoteChargeId = @QuoteChargeId)
					BEGIN
					
					-- StepsCursor
					DECLARE StepsCursor CURSOR READ_ONLY
					FOR
					SELECT Step, SaleUnitPrice, CostUnitPrice, MarkupValue
					FROM QuotePriceSteps
					where QuoteId = @QuoteId AND Tenant = @Tenant AND QuoteChargeId is null
					OPEN StepsCursor FETCH NEXT FROM StepsCursor INTO @Step, @SaleUnitPrice, @CostUnitPrice, @MarkupValue
					WHILE @@FETCH_STATUS = 0
					BEGIN
						
						EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'QuotePriceSteps'
						insert into QuotePriceSteps (Id, Tenant, QuoteId , QuoteChargeId, Step, SaleUnitPrice, CostUnitPrice, MarkupValue)
						values
						(
						@NewId,
						@Tenant,
						@QuoteId,
						@QuoteChargeId,
						@Step,
						@SaleUnitPrice,
						@CostUnitPrice,
						@MarkupValue
						)

					FETCH NEXT FROM StepsCursor INTO @Step, @SaleUnitPrice, @CostUnitPrice, @MarkupValue
					END
					CLOSE StepsCursor
					DEALLOCATE StepsCursor

					END
				END 
	
			FETCH NEXT FROM ChargesCursor INTO @QuoteChargeId, @ChargesTypeId
			END
			CLOSE ChargesCursor
			DEALLOCATE ChargesCursor
		END

		-- Delete Old Steps
		delete from QuotePriceSteps where QuoteId = @QuoteId AND Tenant = @Tenant AND QuoteChargeId is null

	END

	FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @QuoteTypeCode, @TransportModeId, @ShipmentTypeId, @IsFreightBySteps
	END
	CLOSE QuotesCursor
	DEALLOCATE QuotesCursor
END