ALTER TABLE [Customs].[CustomsHouseTypeAdditionals] ADD  CONSTRAINT [UQ_Code_Tenant] UNIQUE NONCLUSTERED 
(

	[Tenant] ASC,
	[Code] ASC
	
)