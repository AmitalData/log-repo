ALTER TABLE [Customs].[InterfaceTenantDefinitions] ADD  CONSTRAINT [UQ_Code_Tenant_Definition] UNIQUE NONCLUSTERED 
(

	[Tenant] ASC,
	[Code] ASC
	
)