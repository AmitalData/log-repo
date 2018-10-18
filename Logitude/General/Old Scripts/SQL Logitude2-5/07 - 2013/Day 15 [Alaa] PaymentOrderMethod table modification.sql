alter table [Customs].[PaymentOrderMethods] add InternalBankId varchar(15)

alter table [Customs].[PaymentOrderMethods] add constraint [PaymentOrderMethod_CustomBank] foreign key ([InternalBankId]) references [Customs].[CustomBanks]([Id]);


