ALTER TABLE [dbo].[ObjectFields] ADD  CONSTRAINT [UQ_ObjectFields_Code_ObjectTableId_Tenant] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC,
	[ObjectTableId] ASC
)