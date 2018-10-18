

alter table ARInvoiceLines add ExchangeRateDate datetime null default null
go

alter table ARInvoices alter column ExchangeRateDate datetime null
go

alter table APInvoices alter column ExchangeRateDate datetime null
go