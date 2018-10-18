Update [Customs].[Declarations] set ProcedureCurrentCode = Null;
go

alter  table [Customs].[Declarations] drop constraint [Declaration_ProcedureCurrent]
go

alter table [Customs].[Declarations] add constraint [Declaration_ItemGovernmentProcedureCurrent] foreign key ([ProcedureCurrentCode]) references [Customs].[ItemGovernmentProcedureTypes]([Code]);
go