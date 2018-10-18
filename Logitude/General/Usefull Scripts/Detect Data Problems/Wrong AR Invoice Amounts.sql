


declare @Tenant as int
declare @InvoiceId as varchar(15)
declare @InvoiceNumber as varchar(50)
declare @CreateDate as date
declare @UpdateDate as date
declare @ApprovedDate as date
declare @LocalCurrencyId as varchar(5)
declare @ProfitCurrencyId as varchar(5)
declare @InvoiceCurrencyId as varchar(5)
declare @SubTotalInLocalCurrency as float
declare @SubTotalInInvoiceCurrency as float
declare @AmountInLocalCurrency as float
declare @AmountInInvoiceCurrency as float
declare @ComputedAmountInLocalCurrency as float
declare @ComputedAmountInInvoiceCurrency as float

declare @MemoryTable table
(
  Id varchar(15) not null,
  InvoiceNumber varchar(50) not null,
  CreateDate datetime null,
  UpdateDate datetime null,
  ApprovedDate datetime null,
  SubTotalLocal float not null,
  AmountInLocal float not null,
  ComputedInLocal float not null,
  SubTotalInvoice float not null,
  AmountInInvoice float not null,
  ComputedInInvoice float not null
)

set @Tenant = 1

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, InvoiceNumber, CreateDate, UpdateDate, ApprovedDate, LocalCurrencyId, ProfitCurrencyId, InvoiceCurrencyId, SubTotalInLocalCurrency, SubTotalInInvoiceCurrency, AmountInLocalCurrency, AmountInInvoiceCurrency
	FROM ARInvoices
	WHERE Tenant = @Tenant
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @InvoiceId, @InvoiceNumber, @CreateDate, @UpdateDate, @ApprovedDate, @LocalCurrencyId, @ProfitCurrencyId, @InvoiceCurrencyId, @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency, @AmountInLocalCurrency, @AmountInInvoiceCurrency
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @ComputedAmountInLocalCurrency = round(@SubTotalInLocalCurrency + (select sum(isnull(LocalVATAmount,0)) from ARInvoiceTotalVATs where Tenant = @Tenant AND ARInvoiceId = @InvoiceId),2)
		set @ComputedAmountInInvoiceCurrency = round(@SubTotalInInvoiceCurrency + (select sum(isnull(InvoiceCurrencyVATAmount,0)) from ARInvoiceTotalVATs where Tenant = @Tenant AND ARInvoiceId = @InvoiceId),2)

		if (@ComputedAmountInLocalCurrency <> @AmountInLocalCurrency OR @ComputedAmountInInvoiceCurrency <> @AmountInInvoiceCurrency)
		begin

			insert into @MemoryTable(Id, InvoiceNumber, SubTotalLocal, AmountInLocal, ComputedInLocal, SubTotalInvoice, AmountInInvoice, ComputedInInvoice, CreateDate, UpdateDate, ApprovedDate)
			values
			(
			@InvoiceId,
			@InvoiceNumber,
			@SubTotalInLocalCurrency,
			@AmountInLocalCurrency,
			@ComputedAmountInLocalCurrency,

			@SubTotalInInvoiceCurrency,
			@AmountInInvoiceCurrency,
			@ComputedAmountInInvoiceCurrency,
			@CreateDate,
			@UpdateDate,
			@ApprovedDate
			)
		end
		
	FETCH NEXT FROM DataCursor INTO @InvoiceId, @InvoiceNumber, @CreateDate, @UpdateDate, @ApprovedDate, @LocalCurrencyId, @ProfitCurrencyId, @InvoiceCurrencyId, @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency, @AmountInLocalCurrency, @AmountInInvoiceCurrency
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor

	select * from @MemoryTable
END

