-- no need for this script 

update arinvoices set  AmountDueInLocalCurrency=ROUND(AmountDue*InvoiceCurrencyExchangeRate,2)
go
update arinvoices set  AmountDueInProfitCurrency=ROUND(AmountDueInLocalCurrency/ProfitCurrencyExchangeRate,2)
go

update APInvoices set  AmountDueInLocalCurrency=ROUND(AmountDue*InvoiceCurrencyExchangeRate,2)
go
update APInvoices set  AmountDueInProfitCurrency=ROUND(AmountDueInLocalCurrency/ProfitCurrencyExchangeRate,2)
go