-- Do not run this script on Production/Cloud versions 

declare @Tenant as int
declare @InvoiceId as varchar(15)
declare @TotalAmountForTaxReportSum as float
declare @TotaVatableAmountForTaxReportSum as float
declare @TotalVATSum as float

BEGIN
		DECLARE DataCursor1 CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM ARInvoices
		OPEN DataCursor1 FETCH NEXT FROM DataCursor1 INTO @InvoiceId, @Tenant
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @TotalAmountForTaxReportSum = round((select sum(isnull(LocalCurrencyAmount,0)) from ARInvoiceLines where ARInvoiceId = @InvoiceId and Tenant = @Tenant and LineActionCode = '1'),2)		
		set @TotaVatableAmountForTaxReportSum = round((select sum(isnull(LocalCurrencyAmount,0)) from ARInvoiceLines where ARInvoiceId = @InvoiceId and Tenant = @Tenant and LineActionCode = '1' and VatPercentage <> 0),2)
		set @TotalVATSum = round((select sum(isnull(LocalVATAmount,0)) from ARInvoiceTotalVATs where Tenant = @Tenant AND ARInvoiceId = @InvoiceId),2)
		update ARInvoices set TotalVAT = @TotalVATSum,TotaVatableAmountForTaxReport = @TotaVatableAmountForTaxReportSum, TotalAmountForTaxReport = @TotalAmountForTaxReportSum where Id = @InvoiceId and Tenant = @Tenant;

		FETCH NEXT FROM DataCursor1 INTO @InvoiceId, @Tenant
		END
		CLOSE DataCursor1
		DEALLOCATE DataCursor1
END