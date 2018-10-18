ALTER TABLE [Customs].[CustomsExchangeRates] ADD  CONSTRAINT [UQ_Id_Tenant_CurrencyCode_Date] UNIQUE NONCLUSTERED 
(

	[Tenant] ASC,
	[Id] ASC,
	[CurrencyTypeCode] ASC,
	[RateDate] ASC
)