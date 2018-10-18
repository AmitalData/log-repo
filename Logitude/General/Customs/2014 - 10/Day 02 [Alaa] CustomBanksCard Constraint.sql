ALTER TABLE Customs.CustomBanksCards ADD  CONSTRAINT [UQ_Tenant_CustomBank_Card] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[CustomBankId] ASC,
	[CardId] ASC
)