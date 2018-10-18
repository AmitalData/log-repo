-- with errors


if not exists (select * from ARInvoiceStatus where Code = 'AC')
begin
	insert into ARInvoiceStatus(Code, Name, SearchFields) values('AC','Auto Credit','AC,Auto Credit')
end

if not exists (select * from ARInvoiceStatus where Code = 'AR')
begin
	insert into ARInvoiceStatus(Code, Name, SearchFields) values('AR','Auto Credited','AR,Auto Credited')
end

update ARInvoices
set StatusCode = 'AC', AmountDue = 0, AmountDueInLocalCurrency = 0, AmountDueInProfitCurrency = 0
where IsAutoCredit = 1
go

update ARInvoices
set StatusCode = 'AR', AmountDue = 0, AmountDueInLocalCurrency = 0, AmountDueInProfitCurrency = 0
where IsCancelled = 1
go