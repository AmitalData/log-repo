
begin transaction
begin

ALTER TABLE [Customs].[Clients] ADD  CONSTRAINT [UQ_Tenant_Code_Clients] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC
)

END
commit transaction
