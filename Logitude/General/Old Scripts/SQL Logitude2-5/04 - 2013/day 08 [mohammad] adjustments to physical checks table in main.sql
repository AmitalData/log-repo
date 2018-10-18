
alter table customs.physicalchecks alter column [OperationCode] varchar(4) null
go

alter table customs.physicalchecks alter column [StatusMessageCode] varchar(4) null
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_Operation] foreign key ([OperationCode]) references [Customs].[PhysicalCheckOperations]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_StatusMessage] foreign key ([StatusMessageCode]) references [Customs].[PhysicalCheckStatusMessages]([Code]);