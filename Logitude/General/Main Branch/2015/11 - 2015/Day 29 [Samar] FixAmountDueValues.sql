
declare @Tenant as int
declare @InvoiceId as varchar(15)
declare @InvoiceAmount as float
declare @ProfitAmount as float
declare @ProfitAmountDue as float
declare @DoUpdate as bit
declare @Status as varchar(2)
declare @Count as int
set @Count = 0

BEGIN 
		DECLARE InvoicesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, AmountInInvoiceCurrency, AmountInProfitCurrency, AmountDueInProfitCurrency, StatusCode
		FROM ARInvoices
		where ProfitCurrencyId = InvoiceCurrencyId
		OPEN InvoicesCursor FETCH NEXT FROM InvoicesCursor INTO @InvoiceId, @Tenant, @InvoiceAmount, @ProfitAmount, @ProfitAmountDue, @Status	
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @DoUpdate = 0

			if(@InvoiceAmount <> @ProfitAmount)
			begin

				set @DoUpdate = 1
				set @ProfitAmount = @InvoiceAmount

			end

			if(@Status != 'PP' and @Status != 'PD')
			begin

				if(@ProfitAmountDue <> @ProfitAmount)
				begin

					set @DoUpdate = 1
					set @ProfitAmountDue = @ProfitAmount

				end
			end

			if(@DoUpdate = 1)
				set @Count = @Count + 1
				update ARInvoices set AmountDueInProfitCurrency = @ProfitAmountDue, AmountInProfitCurrency = @ProfitAmount where Id = @InvoiceId

				FETCH NEXT FROM InvoicesCursor INTO @InvoiceId, @Tenant, @InvoiceAmount, @ProfitAmount, @ProfitAmountDue, @Status	
			END
		CLOSE InvoicesCursor
		DEALLOCATE InvoicesCursor
END