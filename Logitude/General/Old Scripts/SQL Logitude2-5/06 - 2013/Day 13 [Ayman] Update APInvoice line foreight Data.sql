-- Do not run online 
--select * from Currencies

alter table APInvoiceLines add ForiegnCurrencyId varchar(15)
go

alter table APInvoiceLines add ForiegnExchangeRate float
go

alter table APInvoiceLines add ForiegnCurrencyAmount float
go

-- Creating foreign key on [ForiegnCurrencyId] in table 'APInvoiceLines'
ALTER TABLE [dbo].[APInvoiceLines]
ADD CONSTRAINT [FK_APInvoiceLineForiegnCurrency]
    FOREIGN KEY ([ForiegnCurrencyId])
    REFERENCES [dbo].[Currencies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
go

update APInvoiceLines set ForiegnExchangeRate = InvoiceToPayableExchangeRate
go

update APInvoiceLines set ForiegnCurrencyAmount = InvoiceCurrencyAmount
go

update APInvoiceLines set ForiegnCurrencyId = (Select CurrencyId from ShipmentPayables where Id = EntityPayableId)
go

update APInvoiceLines set InvoiceCurrencyAmount = LocalCurrencyAmount / (Select InvoiceCurrencyExchangeRate from APInvoices where Id = APInvoiceId)
go

alter table APInvoiceLines drop column InvoiceToPayableExchangeRate
go

delete from ObjectFields where FieldName = 'InvoiceToPayableExchangeRate'
go

delete from TextCodes where Code like '%InvoiceToPayableExchangeRate%'
go
