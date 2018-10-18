

update ARInvoices set ExchangeRateDate = NULL
go

update APInvoices set ExchangeRateDate = NULL
go

update ARPayments set ExchangeRateDate = NULL
go

update APPayments set PaymentCurrencyExchangeRateDate = NULL
go