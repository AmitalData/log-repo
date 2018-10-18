

if exists (select * from ARInvoices where CancelledByARInvoiceId is not null AND IsCancelled = 1 and StatusCode != 'AR')
begin
	update ARInvoices set StatusCode = 'AR' where CancelledByARInvoiceId is not null AND IsCancelled = 1 and StatusCode != 'AR'
end
GO

if exists (select * from ARInvoices where IsAutoCredit = 1 and StatusCode != 'AC')
begin
	update ARInvoices set StatusCode = 'AC' where IsAutoCredit = 1 and StatusCode != 'AC'
end
GO

update ARInvoices 
set AmountDue = 0,
AmountDueInLocalCurrency = 0,
AmountDueInProfitCurrency = 0
where (StatusCode = 'AR' OR StatusCode = 'AC')
