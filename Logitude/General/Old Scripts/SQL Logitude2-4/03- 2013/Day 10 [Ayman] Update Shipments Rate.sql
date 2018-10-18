
--select * from Tenants where CurrencyId is null
--select * from Tenants where ProfitCurrencyId is null

--select * from shipments where ProfitCurrencyId is null
--select * from shipments where ProfitExchangeRate is null

DECLARE @Tenant AS INT
DECLARE @ShipmentId AS varchar(15)
DECLARE @ProfitCurrencyId AS varchar(15)
DECLARE @TenantLocalCurrencyId AS varchar(15)

DECLARE @MaxDate AS DateTime
DECLARE @MaxPayableDate AS DateTime
DECLARE @MaxReceivableDate AS DateTime
DECLARE @ShipmentCreateDate AS DateTime
DECLARE @RateDate AS DATE
DECLARE @Rate AS FLOAT

DECLARE @MaxLogDateTime AS DATETIME

Declare @sameDate as int
Declare @lessDate as int
Declare @moreDate as int
set @sameDate = 0
set @lessDate = 0
set @moreDate = 0

BEGIN;
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Tenant,Id,ProfitCurrencyId,CreateDateTime
	FROM Shipments
	where ProfitExchangeRate is null
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @Tenant,@ShipmentId,@ProfitCurrencyId,@ShipmentCreateDate
	WHILE @@FETCH_STATUS = 0

		BEGIN;

			-- Get LocalCurrencyId
			SET @TenantLocalCurrencyId = (Select CurrencyId from Tenants where Id = @Tenant)

			-- Get the Date
			iF (EXISTS (SELECT * from ShipmentPayables where ShipmentId = @ShipmentId AND Tenant = @Tenant) AND EXISTS (SELECT * from ShipmentReceivables where ShipmentId = @ShipmentId AND Tenant = @Tenant))
				BEGIN;
					SET @MaxPayableDate = (SELECT MAX(CreateDate) from ShipmentPayables where ShipmentId = @ShipmentId AND Tenant = @Tenant)
					SET @MaxReceivableDate = (SELECT MAX(CreateDate) from ShipmentReceivables where ShipmentId = @ShipmentId AND Tenant = @Tenant)

					SET @MaxDate = @MaxPayableDate
					IF (@MaxReceivableDate > @MaxDate)
						BEGIN;
							SET @MaxDate = @MaxReceivableDate
						END
				END
			
			else iF (EXISTS (SELECT * from ShipmentPayables where ShipmentId = @ShipmentId AND Tenant = @Tenant))
				BEGIN;
					SET @MaxPayableDate = (SELECT MAX(CreateDate) from ShipmentPayables where ShipmentId = @ShipmentId AND Tenant = @Tenant)
					SET @MaxDate = @MaxPayableDate
				END

			else iF (EXISTS (SELECT * from ShipmentReceivables where ShipmentId = @ShipmentId AND Tenant = @Tenant))
				BEGIN;
					
					SET @MaxReceivableDate = (SELECT MAX(CreateDate) from ShipmentReceivables where ShipmentId = @ShipmentId AND Tenant = @Tenant)
					SET @MaxDate = @MaxReceivableDate
				END

			else
				BEGIN;
					SET @MaxDate = @ShipmentCreateDate
				END


			-- Get the Rate
			IF (Select COUNT(Id) from RatesTables where Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId) > 0
				BEGIN;
					
					SET @RateDate = CONVERT(DATE,@MaxDate)
										 
						if (Select COUNT(Id) from RatesTables where ValueDate = @RateDate AND Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId) > 0
						BEGIN;
							set @sameDate = @sameDate + 1
							SET @MaxLogDateTime = (SELECT MAX(LogDateTime) from RatesTables where ValueDate = @RateDate AND Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId)
							SET @Rate = (SELECT MAX(Rate) from RatesTables where LogDateTime = @MaxLogDateTime AND ValueDate = @RateDate AND Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId)
							--print @Rate
						END


						else if (Select COUNT(Id) from RatesTables where ValueDate < @RateDate AND Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId) > 0
						BEGIN;
							set @lessDate = @lessDate + 1
							SET @MaxLogDateTime = (SELECT MAX(LogDateTime) from RatesTables where ValueDate < @RateDate AND Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId)							
							SET @Rate = (SELECT MAX(Rate) from RatesTables where LogDateTime = @MaxLogDateTime AND ValueDate < @RateDate AND Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId)
							--print @Rate
						END


						else if (Select COUNT(Id) from RatesTables where ValueDate > @RateDate AND Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId) > 0
						BEGIN;
							set @moreDate = @moreDate + 1
							SET @MaxLogDateTime = (SELECT MAX(LogDateTime) from RatesTables where ValueDate > @RateDate AND Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId)
							SET @Rate = (SELECT  MAX(Rate) from RatesTables where LogDateTime = @MaxLogDateTime AND ValueDate > @RateDate AND Tenant = @Tenant AND BaseCurrencyId = @TenantLocalCurrencyId AND ForeignCurrencyId = @ProfitCurrencyId)
							--print @Rate
						END

					
				END
		

		Update Shipments set ProfitExchangeRate = @Rate	Where Id = @ShipmentId AND Tenant = @Tenant

		FETCH NEXT FROM ShipmentsCursor INTO @Tenant,@ShipmentId,@ProfitCurrencyId,@ShipmentCreateDate
		END
	
	--print @sameDate
	--print @lessDate
	--print @moreDate
	

	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor	
END