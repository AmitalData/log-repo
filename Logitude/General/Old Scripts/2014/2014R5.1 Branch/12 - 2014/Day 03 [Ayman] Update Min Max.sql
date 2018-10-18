

declare @Tenant as int
declare @QuoteId as varchar(15)
declare @ShipmentId as varchar(15)
declare @MinimumFreightCost as float
declare @MinimumFreightSale as float
declare @ChargeGroupCode as varchar(10)

--DataCursor1: Fill Quote Charges Min/Max
BEGIN
		DECLARE DataCursor2 CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, MinimumFreightCost, MinimumFreightSale
		FROM Quotes
		where QuoteTypeCode = 'P'
		OPEN DataCursor2 FETCH NEXT FROM DataCursor2 INTO @QuoteId, @Tenant, @MinimumFreightCost, @MinimumFreightSale
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if (@MinimumFreightCost is not null)
			begin
							
				update QuoteCharges
				set CostMinAmount = @MinimumFreightCost
				where Tenant = @Tenant
				and QuoteId = @QuoteId
				and (ChargesTypeId in (select Id from ChargesTypes where Tenant = @Tenant and ChargesGroupCode = 'FRT'))
							 
			end

			if (@MinimumFreightSale is not null)
			begin

				update QuoteCharges
				set SaleMinAmount = @MinimumFreightSale
				where Tenant = @Tenant
				and QuoteId = @QuoteId
				and (ChargesTypeId in (select Id from ChargesTypes where Tenant = @Tenant and ChargesGroupCode = 'FRT'))

			end

			FETCH NEXT FROM DataCursor2 INTO @QuoteId, @Tenant, @MinimumFreightCost, @MinimumFreightSale
			END
		CLOSE DataCursor2
		DEALLOCATE DataCursor2
END

--DataCursor2: Fill Receivables & Payables Min/Max
BEGIN
		DECLARE DataCursor3 CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, QuoteId
		FROM Shipments
		where QuoteId is not null
		OPEN DataCursor3 FETCH NEXT FROM DataCursor3 INTO @ShipmentId, @Tenant, @QuoteId
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if exists (select * from Quotes where Tenant = @Tenant and Id = @QuoteId and QuoteTypeCode = 'P')
			begin

				select
				@MinimumFreightCost = MinimumFreightCost,
				@MinimumFreightSale = MinimumFreightSale
				from Quotes
				where Tenant = @Tenant and Id = @QuoteId

				update ShipmentPayables
				set QuoteCostMinPrice = @MinimumFreightCost
				where Tenant = @Tenant
				and ShipmentId = @ShipmentId
				and (ChargesTypeId in (select Id from ChargesTypes where Tenant = @Tenant and ChargesGroupCode = 'FRT'))

				update ShipmentReceivables
				set QuoteSaleMinPrice = @MinimumFreightSale
				where Tenant = @Tenant
				and ShipmentId = @ShipmentId
				and (ChargesTypeId in (select Id from ChargesTypes where Tenant = @Tenant and ChargesGroupCode = 'FRT'))

			end

			FETCH NEXT FROM DataCursor3 INTO @ShipmentId, @Tenant, @QuoteId
			END
		CLOSE DataCursor3
		DEALLOCATE DataCursor3
END

