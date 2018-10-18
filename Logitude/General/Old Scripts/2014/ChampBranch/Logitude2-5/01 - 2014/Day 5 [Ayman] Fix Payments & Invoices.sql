

-- This will delete duplicate rows, except first row
DELETE FROM ARInvoicePayments WHERE Id NOT IN (SELECT MIN(Id) FROM ARInvoicePayments GROUP BY ARPaymentId,ARInvoiceId)
GO

-- This will delete duplicate rows, except first row
DELETE FROM APInvoicePayments WHERE Id NOT IN (SELECT MIN(Id) FROM APInvoicePayments GROUP BY APPaymentId,APInvoiceId)
GO

declare @Tenant as int
declare @EntityId as varchar(15)
declare @EntityAmount as float
declare @ConnectedAmount as float
declare @EntityIsClosed as bit
declare @EntityStatusCode as varchar(2)
declare @OpenAmount as float
declare @AmountDue as float
declare @AmountDueLocal as float
declare @AmountDueProfit as float
declare @ExchangeRate as float
declare @ProfitExchangeRate as float

-- Fix [AR] Payments OpenAmount
BEGIN
	DECLARE ARPaymentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, AmountInPaymentCurrency, StatusCode
	FROM ARPayments
	OPEN ARPaymentsCursor FETCH NEXT FROM ARPaymentsCursor INTO @EntityId, @Tenant, @EntityAmount, @EntityStatusCode
	WHILE @@FETCH_STATUS = 0
		BEGIN

			if @EntityAmount is null
			set @EntityAmount = 0
							
			set @ConnectedAmount = 0
			if exists (select Id from ARInvoicePayments where ARPaymentId = @EntityId and Tenant = @Tenant)
			set @ConnectedAmount = (select sum(Amount) from (select min(ForeignAmount) as Amount FROM ARInvoicePayments where ARPaymentId = @EntityId and Tenant = @Tenant GROUP BY ARPaymentId,ARInvoiceId) as Query)

			set @EntityAmount = ROUND(@EntityAmount,2)
			set @ConnectedAmount = ROUND(@ConnectedAmount,2)					
			set @OpenAmount = ROUND(@EntityAmount - @ConnectedAmount,2)

			if @OpenAmount = 0
			begin
				set @EntityIsClosed = 1
				if @EntityStatusCode = 'AD'
				set @EntityStatusCode = 'CL'
			end

			else
			begin
				set @EntityIsClosed = 0
				if @EntityStatusCode != 'VD' And @EntityStatusCode != 'DR'
				set @EntityStatusCode = 'AD'
			end

			update ARPayments
			set OpenAmount = @OpenAmount, IsClosed = @EntityIsClosed, StatusCode = @EntityStatusCode
			where Id = @EntityId and Tenant = @Tenant

		FETCH NEXT FROM ARPaymentsCursor INTO @EntityId, @Tenant, @EntityAmount, @EntityStatusCode
		END
	CLOSE ARPaymentsCursor
	DEALLOCATE ARPaymentsCursor
END

-- Fix [AP] Payments OpenAmount
BEGIN
	DECLARE APPaymentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, AmountInPaymentCurrency, StatusCode
	FROM APPayments
	OPEN APPaymentsCursor FETCH NEXT FROM APPaymentsCursor INTO @EntityId, @Tenant, @EntityAmount, @EntityStatusCode
	WHILE @@FETCH_STATUS = 0
		BEGIN
		
			if @EntityAmount is null
			set @EntityAmount = 0

			set @ConnectedAmount = 0
			if exists (select Id from APInvoicePayments where APPaymentId = @EntityId and Tenant = @Tenant)
			set @ConnectedAmount = (select sum(Amount) from (select min(ForeignAmount) as Amount FROM APInvoicePayments where APPaymentId = @EntityId and Tenant = @Tenant GROUP BY APPaymentId,APInvoiceId) as Query)

			set @EntityAmount = ROUND(@EntityAmount,2)
			set @ConnectedAmount = ROUND(@ConnectedAmount,2)								
			set @OpenAmount = ROUND(@EntityAmount - @ConnectedAmount,2)

			if @OpenAmount = 0
			begin
				set @EntityIsClosed = 1
				if @EntityStatusCode = 'AD'
				set @EntityStatusCode = 'CL'
			end

			else
			begin
				set @EntityIsClosed = 0
				if @EntityStatusCode != 'VD' And @EntityStatusCode != 'DR'
				set @EntityStatusCode = 'AD'
			end

			update APPayments
			set OpenAmount = @OpenAmount, IsClosed = @EntityIsClosed, StatusCode = @EntityStatusCode
			where Id = @EntityId and Tenant = @Tenant

		FETCH NEXT FROM APPaymentsCursor INTO @EntityId, @Tenant, @EntityAmount, @EntityStatusCode
		END
	CLOSE APPaymentsCursor
	DEALLOCATE APPaymentsCursor
END

-- Fix [AR] Invoices AmountDue
BEGIN
	DECLARE ARInvoicesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, AmountInInvoiceCurrency, StatusCode, InvoiceCurrencyExchangeRate, ProfitCurrencyExchangeRate
	FROM ARInvoices
	OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @EntityId, @Tenant, @EntityAmount, @EntityStatusCode, @ExchangeRate, @ProfitExchangeRate
	WHILE @@FETCH_STATUS = 0
		BEGIN
				
			if @EntityAmount is null
			set @EntityAmount = 0

			set @ConnectedAmount = 0
			if exists (select Id from ARInvoicePayments where ARInvoiceId = @EntityId and Tenant = @Tenant)
			set @ConnectedAmount = (select sum(Amount) from (select min(ForeignAmount) as Amount FROM ARInvoicePayments where ARInvoiceId = @EntityId and Tenant = @Tenant GROUP BY ARPaymentId,ARInvoiceId) as Query)

			set @EntityAmount = ROUND(@EntityAmount,2)
			set @ConnectedAmount = ROUND(@ConnectedAmount,2)

			if (@EntityStatusCode = 'PD' OR @EntityStatusCode = 'PP')
			begin
				if @ConnectedAmount = 0
				set @EntityStatusCode = 'AD'
				else set @EntityStatusCode = 'PP'
			end

			if @ProfitExchangeRate = 0
			set @ProfitExchangeRate = 1

			set @AmountDue = ROUND(@EntityAmount - @ConnectedAmount,2)
			set @AmountDueLocal = Round(@AmountDue * @ExchangeRate, 2)
			set @AmountDueProfit = Round(@AmountDueLocal / @ProfitExchangeRate, 2)

			if @AmountDue = 0
			begin
				set @EntityIsClosed = 1
				set @EntityStatusCode = 'PD'
			end

            else if (@AmountDue > 0 AND @AmountDue < @EntityAmount)
            begin
				set @EntityIsClosed = 0;
				set @EntityStatusCode = 'PP'
            end

            else if (@AmountDue < 0 AND @AmountDue > @EntityAmount)
            begin
				set @EntityIsClosed = 0;
				set @EntityStatusCode = 'PP'
            end

			else set @EntityIsClosed = 0

			update ARInvoices
			set 
			AmountInInvoiceCurrency = @EntityAmount,
			AmountDue = @AmountDue,
			AmountDueInLocalCurrency = @AmountDueLocal,
			AmountDueInProfitCurrency = @AmountDueProfit,
			IsClosed = @EntityIsClosed,
			StatusCode = @EntityStatusCode
			where Id = @EntityId and Tenant = @Tenant

		FETCH NEXT FROM ARInvoicesCursor INTO @EntityId, @Tenant, @EntityAmount, @EntityStatusCode, @ExchangeRate, @ProfitExchangeRate
		END
	CLOSE ARInvoicesCursor
	DEALLOCATE ARInvoicesCursor
END

-- Fix [AP] Invoices AmountDue
BEGIN
	DECLARE APInvoicesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, AmountInInvoiceCurrency, StatusCode, InvoiceCurrencyExchangeRate, ProfitCurrencyExchangeRate
	FROM APInvoices
	OPEN APInvoicesCursor FETCH NEXT FROM APInvoicesCursor INTO @EntityId, @Tenant, @EntityAmount, @EntityStatusCode, @ExchangeRate, @ProfitExchangeRate
	WHILE @@FETCH_STATUS = 0
		BEGIN
				
			if @EntityAmount is null
			set @EntityAmount = 0

			set @ConnectedAmount = 0
			if exists (select Id from APInvoicePayments where APInvoiceId = @EntityId and Tenant = @Tenant)
			set @ConnectedAmount = (select sum(Amount) from (select min(ForeignAmount) as Amount FROM APInvoicePayments where APInvoiceId = @EntityId and Tenant = @Tenant GROUP BY APPaymentId,APInvoiceId) as Query)

			set @EntityAmount = ROUND(@EntityAmount,2)
			set @ConnectedAmount = ROUND(@ConnectedAmount,2)

			if (@EntityStatusCode = 'PD' OR @EntityStatusCode = 'PP')
			begin
				if @ConnectedAmount = 0
				set @EntityStatusCode = 'AD'
				else set @EntityStatusCode = 'PP'
			end

			if @ProfitExchangeRate = 0
			set @ProfitExchangeRate = 1

			set @AmountDue = ROUND(@EntityAmount - @ConnectedAmount,2)
			set @AmountDueLocal = Round(@AmountDue * @ExchangeRate, 2)
			set @AmountDueProfit = Round(@AmountDueLocal / @ProfitExchangeRate, 2)

			if @AmountDue = 0
			begin
				set @EntityIsClosed = 1
				set @EntityStatusCode = 'PD'
			end

            else if (@AmountDue > 0 AND @AmountDue < @EntityAmount)
            begin
				set @EntityIsClosed = 0;
				set @EntityStatusCode = 'PP'
            end

            else if (@AmountDue < 0 AND @AmountDue > @EntityAmount)
            begin
				set @EntityIsClosed = 0;
				set @EntityStatusCode = 'PP'
            end

			else set @EntityIsClosed = 0

			update APInvoices
			set 
			AmountInInvoiceCurrency = @EntityAmount,
			AmountDue = @AmountDue,
			AmountDueInLocalCurrency = @AmountDueLocal,
			AmountDueInProfitCurrency = @AmountDueProfit,
			IsClosed = @EntityIsClosed,
			StatusCode = @EntityStatusCode
			where Id = @EntityId and Tenant = @Tenant

		FETCH NEXT FROM APInvoicesCursor INTO @EntityId, @Tenant, @EntityAmount, @EntityStatusCode, @ExchangeRate, @ProfitExchangeRate
		END
	CLOSE APInvoicesCursor
	DEALLOCATE APInvoicesCursor
END