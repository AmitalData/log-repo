declare @Tenant as int
declare @Id as varchar(15)
declare @OpenAmountInLocalCurrency as float
declare @OpenAmount as float
declare @ExchangeRate as float


BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id,OpenAmount,PaymentCurrencyExchangeRate, Tenant 
	FROM ARPayments
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id,@OpenAmount,@ExchangeRate, @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @OpenAmountInLocalCurrency = round((@OpenAmount * @ExchangeRate),2)
		update ARPayments set OpenAmountInLocalCurrency = @OpenAmountInLocalCurrency where Id=@Id and Tenant=@Tenant

	FETCH NEXT FROM DataCursor INTO @Id,@OpenAmount,@ExchangeRate, @Tenant
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor

END

