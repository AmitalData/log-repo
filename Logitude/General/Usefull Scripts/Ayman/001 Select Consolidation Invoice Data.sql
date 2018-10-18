
-- Consolidation InvoiceNumber
declare @Tenant as int
declare @InvoiceNumber as varchar(20)

set @Tenant = 1
set @InvoiceNumber = 'CON1256'

-- Consolidation Invoice
select Id, Tenant, AmountInInvoiceCurrency, AmountInLocalCurrency, AmountInProfitCurrency 
from ARInvoices
where Tenant = @Tenant AND InvoiceNumber = @InvoiceNumber

-- Connected Constituent Invoices
select Id, Tenant, AmountInInvoiceCurrency, AmountInLocalCurrency, AmountInProfitCurrency, InvoiceNumber
from ARInvoices
where Tenant = @Tenant AND ConsolidationInvoiceId = (select Id from ARInvoices where Tenant = @Tenant AND InvoiceNumber = @InvoiceNumber)

-- All Connected Constituent Invoices lines
select Id, Tenant, InvoiceCurrencyAmount, LocalCurrencyAmount, ProfitCurrencyAmount, ForiegnCurrencyAmount
from ARInvoiceLines
where Tenant = @Tenant AND ARInvoiceId in (select Id from ARInvoices where Tenant = @Tenant AND ConsolidationInvoiceId = (select Id from ARInvoices where Tenant = @Tenant AND InvoiceNumber = @InvoiceNumber))

-- Consolidation Invoice lines (the grouped by)
select Id, Tenant, InvoiceCurrencyAmount, LocalCurrencyAmount, ProfitCurrencyAmount, ForiegnCurrencyAmount
from ARInvoiceLines
where Tenant = @Tenant AND ARInvoiceId = (select Id from ARInvoices where Tenant = @Tenant AND InvoiceNumber = @InvoiceNumber)

