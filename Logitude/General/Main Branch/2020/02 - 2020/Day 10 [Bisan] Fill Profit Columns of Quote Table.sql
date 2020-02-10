	declare @Tenant as int
	declare @QuoteId as varchar(15)
	declare @ProfitCurrencyId as varchar(15)
	declare @LocalCurrencyId as varchar(15)
	declare @QuoteDate as datetime
	declare @ProfitCurrencyRate as float
	declare @AmountInLocal as float
	declare @AmountInProfit as float
	declare @EstimateProfit as float
	declare @ExchangeRate as float


	DECLARE QuotesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, OpenDate, EstimateProfit, ExchangeRate
	FROM Quotes	
	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @QuoteDate, @EstimateProfit, @ExchangeRate
	WHILE @@FETCH_STATUS = 0
	BEGIN

		SELECT @LocalCurrencyId = CurrencyId, @ProfitCurrencyId = ProfitCurrencyId FROM Tenants where Id = @Tenant

		set @ProfitCurrencyRate = dbo.GetLastRate(@Tenant, @QuoteDate, @LocalCurrencyId, @ProfitCurrencyId)
		set @AmountInLocal = round((@EstimateProfit * @ExchangeRate),2)
		set @AmountInProfit = round((@AmountInLocal / @ProfitCurrencyRate),2)

		update Quotes set
		EstimatedProfitInLocal = @AmountInLocal,
		EstimatedProfitInProfit = @AmountInProfit,
		ProfitCurrencyId = @ProfitCurrencyId,
		ProfitExchangeRate = @ProfitCurrencyRate

	FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @QuoteDate, @EstimateProfit, @ExchangeRate
	END
	CLOSE QuotesCursor
	DEALLOCATE QuotesCursor