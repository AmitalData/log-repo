declare @Tenant as int
declare @InvoiceId as varchar(15)
declare @TotaVatableAmountForTaxReportSum as float


BEGIN
		DECLARE DataCursor1 CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM ARInvoices
		OPEN DataCursor1 FETCH NEXT FROM DataCursor1 INTO @InvoiceId, @Tenant
		WHILE @@FETCH_STATUS = 0
		BEGIN
		set @TotaVatableAmountForTaxReportSum =  round((select sum(isnull(LocalCurrencyAmount,0)) from ARInvoiceLines where ARInvoiceId = @InvoiceId and Tenant = @Tenant and LineActionCode = '1' and VatPercentage <> 0 and Tenant in (select Id from Tenants where AccountingActivated =1)),2)
        update ARInvoices set  TotaVatableAmountForTaxReport = @TotaVatableAmountForTaxReportSum where Id = @InvoiceId and Tenant = @Tenant;
		FETCH NEXT FROM DataCursor1 INTO @InvoiceId, @Tenant
		END
		CLOSE DataCursor1
		DEALLOCATE DataCursor1
END
