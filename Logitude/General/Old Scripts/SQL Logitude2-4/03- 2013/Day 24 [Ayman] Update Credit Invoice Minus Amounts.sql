

declare @InvoiceId as varchar(15)
declare @Tenant as int

declare @InvoiceAmount as float
declare @InvoiceAmountLocal as float
declare @InvoiceAmountProfit as float
declare @InvoiceAmountDue as float
declare @InvoiceAmountDueLocal as float
declare @InvoiceAmountDueProfit as float
declare @SubTotalAmount as float
declare @SubTotalAmountLocal as float

declare @InvoiceLineId as varchar(15)
declare @LineForiegnAmount as float
declare @LineLocalAmount as float
declare @LineProfitAmount as float
declare @LineInvoiceAmount as float
declare @LineUnitPrice as float

declare @TotalVatId as varchar(15)
declare @VatableAmount as float
declare @VatableAmountLocal as float
declare @VatableAmountProfit as float

declare @InvoicePaymentId as varchar(15)
declare @InvoicePaymentAmount as float
declare @InvoicePaymentAmountLocal as float

BEGIN;

	DECLARE ARInvoicesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant, AmountInInvoiceCurrency, AmountInLocalCurrency, AmountInProfitCurrency, AmountDue, AmountDueInLocalCurrency, AmountDueInProfitCurrency, SubTotalInInvoiceCurrency, SubTotalInLocalCurrency
	FROM ARInvoices
	where ARInvoiceTypeCode = 'CD'
	OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @InvoiceId,@Tenant,@InvoiceAmount,@InvoiceAmountLocal,@InvoiceAmountProfit,@InvoiceAmountDue,@InvoiceAmountDueLocal,@InvoiceAmountDueProfit,@SubTotalAmount,@SubTotalAmountLocal
	WHILE @@FETCH_STATUS = 0
	BEGIN;

	if(@InvoiceAmount > 0) set @InvoiceAmount = @InvoiceAmount * -1
	if(@InvoiceAmountLocal > 0) set @InvoiceAmountLocal = @InvoiceAmountLocal * -1
	if(@InvoiceAmountProfit > 0) set @InvoiceAmountProfit = @InvoiceAmountProfit * -1
	if(@InvoiceAmountDue > 0) set @InvoiceAmountDue = @InvoiceAmountDue * -1
	if(@InvoiceAmountDueLocal > 0) set @InvoiceAmountDueLocal = @InvoiceAmountDueLocal * -1
	if(@InvoiceAmountDueProfit > 0) set @InvoiceAmountDueProfit = @InvoiceAmountDueProfit * -1
	if(@SubTotalAmount > 0) set @SubTotalAmount = @SubTotalAmount * -1
	if(@SubTotalAmountLocal > 0) set @SubTotalAmountLocal = @SubTotalAmountLocal * -1

	update ARInvoices set
	AmountInInvoiceCurrency = @InvoiceAmount,
	AmountInLocalCurrency = @InvoiceAmountLocal,
	AmountInProfitCurrency = @InvoiceAmountProfit,
	AmountDue = @InvoiceAmountDue,
	AmountDueInLocalCurrency = @InvoiceAmountDueLocal,
	AmountDueInProfitCurrency = @InvoiceAmountDueProfit,
	SubTotalInInvoiceCurrency = @SubTotalAmount,
	SubTotalInLocalCurrency = @SubTotalAmountLocal
	where Id = @InvoiceId AND Tenant = @Tenant


			-- Update InvoiceLines
			DECLARE ARInvoiceLinesCursor CURSOR READ_ONLY
			FOR
			SELECT Id, ForiegnCurrencyAmount, LocalCurrencyAmount, ProfitCurrencyAmount, InvoiceCurrencyAmount,unitprice
			FROM ARInvoiceLines
			where ARInvoiceId = @InvoiceId AND Tenant = @Tenant
			OPEN ARInvoiceLinesCursor FETCH NEXT FROM ARInvoiceLinesCursor INTO @InvoiceLineId,@LineForiegnAmount,@LineLocalAmount,@LineProfitAmount,@LineInvoiceAmount,@LineUnitPrice
			WHILE @@FETCH_STATUS = 0
				BEGIN;

					if(@LineForiegnAmount > 0) set @LineForiegnAmount = @LineForiegnAmount * -1
					if(@LineLocalAmount > 0) set @LineLocalAmount = @LineLocalAmount * -1
					if(@LineProfitAmount > 0) set @LineProfitAmount = @LineProfitAmount * -1
					if(@LineInvoiceAmount > 0) set @LineInvoiceAmount = @LineInvoiceAmount * -1
					if(@LineUnitPrice > 0) set @LineUnitPrice = @LineUnitPrice * -1

					update ARInvoiceLines set
					ForiegnCurrencyAmount = @LineForiegnAmount,
					LocalCurrencyAmount = @LineLocalAmount,
					ProfitCurrencyAmount = @LineProfitAmount,
					InvoiceCurrencyAmount = @LineInvoiceAmount,
					UnitPrice = @LineUnitPrice
					where Id = @InvoiceLineId AND ARInvoiceId = @InvoiceId AND Tenant = @Tenant

				FETCH NEXT FROM ARInvoiceLinesCursor INTO @InvoiceLineId,@LineForiegnAmount,@LineLocalAmount,@LineProfitAmount,@LineInvoiceAmount,@LineUnitPrice
				END
			CLOSE ARInvoiceLinesCursor
			DEALLOCATE ARInvoiceLinesCursor	



			-- Update TotalVats
			DECLARE TotalVatsCursor CURSOR READ_ONLY
			FOR
			SELECT Id, InvoiceCurrencyVatableAmount, LocalVatableAmount, ProfitVatableAmount
			FROM ARInvoiceTotalVATs
			where ARInvoiceId = @InvoiceId AND Tenant = @Tenant
			OPEN TotalVatsCursor FETCH NEXT FROM TotalVatsCursor INTO @TotalVatId,@VatableAmount,@VatableAmountLocal,@VatableAmountProfit
			WHILE @@FETCH_STATUS = 0
				BEGIN;

					if(@VatableAmount > 0) set @VatableAmount = @VatableAmount * -1
					if(@VatableAmountLocal > 0) set @VatableAmountLocal = @VatableAmountLocal * -1
					if(@VatableAmountProfit > 0) set @VatableAmountProfit = @VatableAmountProfit * -1

					update ARInvoiceTotalVATs set
					InvoiceCurrencyVatableAmount = @VatableAmount,
					LocalVatableAmount = @VatableAmountLocal,
					ProfitVatableAmount = @VatableAmountProfit
					where Id = @TotalVatId AND ARInvoiceId = @InvoiceId AND Tenant = @Tenant

				FETCH NEXT FROM TotalVatsCursor INTO @TotalVatId,@VatableAmount,@VatableAmountLocal,@VatableAmountProfit
				END
			CLOSE TotalVatsCursor
			DEALLOCATE TotalVatsCursor	



			-- Update InvoicePayment
			DECLARE InvoicePaymentsCursor CURSOR READ_ONLY
			FOR
			SELECT Id, ForeignAmount, LocalAmount
			FROM ARInvoicePayments
			where ARInvoiceId = @InvoiceId AND Tenant = @Tenant
			OPEN InvoicePaymentsCursor FETCH NEXT FROM InvoicePaymentsCursor INTO @InvoicePaymentId,@InvoicePaymentAmount,@InvoicePaymentAmountLocal
			WHILE @@FETCH_STATUS = 0
				BEGIN;

					if(@InvoicePaymentAmount > 0) set @InvoicePaymentAmount = @InvoicePaymentAmount * -1
					if(@InvoicePaymentAmountLocal > 0) set @InvoicePaymentAmountLocal = @InvoicePaymentAmountLocal * -1

					update ARInvoicePayments set
					ForeignAmount = @InvoicePaymentAmount,
					LocalAmount = @InvoicePaymentAmountLocal
					where Id = @InvoicePaymentId AND ARInvoiceId = @InvoiceId AND Tenant = @Tenant

				FETCH NEXT FROM InvoicePaymentsCursor INTO @InvoicePaymentId,@InvoicePaymentAmount,@InvoicePaymentAmountLocal
				END
			CLOSE InvoicePaymentsCursor
			DEALLOCATE InvoicePaymentsCursor	

	FETCH NEXT FROM ARInvoicesCursor INTO @InvoiceId,@Tenant,@InvoiceAmount,@InvoiceAmountLocal,@InvoiceAmountProfit,@InvoiceAmountDue,@InvoiceAmountDueLocal,@InvoiceAmountDueProfit,@SubTotalAmount,@SubTotalAmountLocal
	END
	CLOSE ARInvoicesCursor
	DEALLOCATE ARInvoicesCursor	
END