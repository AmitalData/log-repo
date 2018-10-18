 --please excute line1 before migration and the rest of the script after migration
ALTER TABLE [Customs].[CustomBanks] drop  CONSTRAINT [UQ_Tenant_InternalCode_CustomBanks]
GO


ALTER TABLE [Customs].[CustomBanks] ADD  CONSTRAINT [UQ_Tenant_InternalCode_CustomBanks] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[InternalCode] ASC
)


