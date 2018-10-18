
ALTER TABLE Customs.PhysicalChecks ADD  CONSTRAINT [UQ_Tenant_CheckId_PhysicalChecks] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[CheckId] ASC
	
)