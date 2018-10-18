
alter table [Customs].[ClientAddresses]  alter column LocalApartment decimal(4, 0) null

alter table [Customs].[ClientAddresses] alter column LocalPOBox [decimal](9, 0) null

alter table [Customs].[ClientAddresses] alter column [LocalPostalCode] [decimal](7, 0) null
alter table [Customs].[ClientAddresses] alter column [LocalHouseNumber] [decimal](4, 0) null

alter table [Customs].[PaymentOrderProtestReasons] alter column [GoodsItemLineNumber] [decimal](5, 0) null

alter table [Customs].[PaymentOrderProtestReasons] alter column [AmountInDispute] [decimal](16, 2) null