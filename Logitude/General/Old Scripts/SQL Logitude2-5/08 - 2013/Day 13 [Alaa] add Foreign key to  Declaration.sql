alter table [Customs].[Declarations] drop constraint [Declaration_ItemGovernmentProcedureCurrent]
GO
update [Customs].[Declarations] set ProcedureCurrentCode = null
go
alter table [Customs].[Declarations] add constraint [Declaration_GovernmentProcedureCurrent] foreign key ([ProcedureCurrentCode]) references [Customs].[GovernmentProcedureTypes]([Code]);
go