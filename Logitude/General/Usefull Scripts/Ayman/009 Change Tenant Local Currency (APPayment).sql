
-- Update (A/P) Payments
-- ProfitCurrencyExchangeRate , AmountInProfitCurrency (Not used)

SET NOCOUNT ON

IF OBJECT_ID (N'dbo.GetDateFormate', N'FN') IS NOT NULL
    DROP FUNCTION dbo.GetDateFormate;
GO

IF OBJECT_ID (N'dbo.GetLastRate', N'FN') IS NOT NULL
    DROP FUNCTION dbo.GetLastRate;
GO

CREATE FUNCTION dbo.GetDateFormate (@dateTime datetime)
RETURNS date
WITH EXECUTE AS CALLER
AS
BEGIN
     DECLARE @Result date;
       SET @Result = dateadd(dd, datediff(dd, 0, @dateTime), 0)
     RETURN(@Result);
END;
GO

CREATE FUNCTION dbo.GetLastRate (@Tenant int, @DateTime datetime, @BaseCurrencyId varchar(15), @ForeignCurrencyId varchar(15))
RETURNS float
WITH EXECUTE AS CALLER
AS
BEGIN
     DECLARE @Result float;

       if (@BaseCurrencyId = @ForeignCurrencyId)
       BEGIN
       set @Result = 1
       END

       else
       BEGIN
              declare @ValueDate as date
              set @ValueDate = dbo.GetDateFormate(@DateTime)
       
               if exists (select * from RatesTables where BaseCurrencyId = @BaseCurrencyId and ForeignCurrencyId = @ForeignCurrencyId and Tenant = @Tenant)
              begin

                     if exists (select * from RatesTables where BaseCurrencyId = @BaseCurrencyId and ForeignCurrencyId = @ForeignCurrencyId and Tenant = @Tenant and ValueDate = @ValueDate)
                     begin
                           set @Result = (select top(1) Rate from RatesTables 
                                                       where BaseCurrencyId = @BaseCurrencyId 
                                                       and ForeignCurrencyId = @ForeignCurrencyId
                                                       and Tenant = @Tenant 
                                                       and ValueDate = @ValueDate
                                                       order by LogDateTime DESC
                                                       )
                     end

                     else if exists (select * from RatesTables where BaseCurrencyId = @BaseCurrencyId and ForeignCurrencyId = @ForeignCurrencyId and Tenant = @Tenant and ValueDate < @ValueDate)
                     begin
                           set @Result = (select top(1) Rate from RatesTables 
                                                       where BaseCurrencyId = @BaseCurrencyId 
                                                       and ForeignCurrencyId = @ForeignCurrencyId
                                                       and Tenant = @Tenant 
                                                       and ValueDate < @ValueDate
                                                       order by LogDateTime DESC
                                                       )
                     end

                     else
                     begin
                           set @Result = (select top(1) Rate from RatesTables 
                                                       where BaseCurrencyId = @BaseCurrencyId 
                                                       and ForeignCurrencyId = @ForeignCurrencyId
                                                       and Tenant = @Tenant
                                                       order by LogDateTime DESC
                                                       )
                     end

              end
       END
     RETURN(@Result);
END;
GO

declare @Tenant as int
declare @NewLocalCurrencyCode as varchar(30)
declare @IsRunningScript as bit
set @IsRunningScript = 0

-- Insert your variables (NIS:USD:EUR)
set @Tenant = 99999
set @NewLocalCurrencyCode = '99999'

declare @LocalCurrencyId as varchar(15)
set @LocalCurrencyId = (select Id from Currencies where Tenant = @Tenant and Code = @NewLocalCurrencyCode)

if not exists (select * from Tenants where Id = @Tenant)
begin
       print 'Error 01: This Tenant is not exists!'
end

else if not exists (select * from Currencies where Tenant = @Tenant and Id = @LocalCurrencyId)
begin
       print 'Error 02: This Currency is not exists!'
end

else
begin
		
	declare @CurrencyId as varchar(15)
	declare @CurrencyName as varchar(40)
	declare @AllCurrenciesRatesExists as bit
	set @AllCurrenciesRatesExists = 1

	DECLARE CurrenciesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName
	FROM Currencies
	WHERE Tenant = @Tenant
	OPEN CurrenciesCursor FETCH NEXT FROM CurrenciesCursor INTO @CurrencyId, @CurrencyName
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if (@CurrencyId != @LocalCurrencyId) AND not exists (select * from RatesTables where Tenant = @Tenant and BaseCurrencyId = @LocalCurrencyId and ForeignCurrencyId = @CurrencyId)
		begin
			set @AllCurrenciesRatesExists = 0
			print 'Error 03: ' + @CurrencyName + ' Currency has no rates in this Tenant'
		end		

	FETCH NEXT FROM CurrenciesCursor INTO @CurrencyId, @CurrencyName	
	END
	CLOSE CurrenciesCursor
	DEALLOCATE CurrenciesCursor

	IF (@AllCurrenciesRatesExists = 1)
	set @IsRunningScript = 1
end

IF (@IsRunningScript = 1)
BEGIN

	print 'Start...'	
	update Tenants set CurrencyId = @LocalCurrencyId where Id = @Tenant and CurrencyId <> @LocalCurrencyId

	-- Payments
	BEGIN

		update APPayments set LocalCurrencyId = @LocalCurrencyId where Tenant = @Tenant
		update APPayments set RegisterDate = UpdateDate where RegisterDate is null and Tenant = @Tenant
		update APPayments set RegisterDate = CreateDate where RegisterDate is null and Tenant = @Tenant

		declare @PaymentId as varchar(15)
		declare @PaymentDate as datetime
		declare @PaymentCurrencyId as varchar(15)
		declare @PaymentCurrencyRate as float

		declare @Amount as float
		declare @AmountLocal as float

		DECLARE PaymentsCursor CURSOR READ_ONLY
		FOR
		SELECT Id, RegisterDate, PaymentCurrencyId, PaymentCurrencyExchangeRate, AmountInPaymentCurrency
		FROM APPayments
		WHERE Tenant = @Tenant
		OPEN PaymentsCursor FETCH NEXT FROM PaymentsCursor INTO @PaymentId, @PaymentDate, @PaymentCurrencyId, @PaymentCurrencyRate, @Amount
		WHILE @@FETCH_STATUS = 0
		BEGIN

			-- Rate + AmountLocal
			if (@PaymentCurrencyId = @LocalCurrencyId)
			begin
				set @PaymentCurrencyRate = 1
				set @AmountLocal = @Amount
			end
			else
			begin
				set @PaymentCurrencyRate = dbo.GetLastRate(@Tenant, @PaymentDate, @LocalCurrencyId, @PaymentCurrencyId)
				set @AmountLocal = @Amount * @PaymentCurrencyRate
			end

			update APPayments
			set
			PaymentCurrencyExchangeRate = Round(@PaymentCurrencyRate, 5),
			AmountInLocalCurrency = Round(@AmountLocal, 5)
			where Id = @PaymentId and Tenant = @Tenant

		FETCH NEXT FROM PaymentsCursor INTO @PaymentId, @PaymentDate, @PaymentCurrencyId, @PaymentCurrencyRate, @Amount
		END
		CLOSE PaymentsCursor
		DEALLOCATE PaymentsCursor
	END

	print 'End Successfully...'
	SET NOCOUNT OFF
END

