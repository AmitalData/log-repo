ALTER TABLE [dbo].[JournalActionTypes] ADD  CONSTRAINT [UQ_Code_Tenant] UNIQUE NONCLUSTERED
(
	[Code] ASC,
	[Tenant] ASC
)