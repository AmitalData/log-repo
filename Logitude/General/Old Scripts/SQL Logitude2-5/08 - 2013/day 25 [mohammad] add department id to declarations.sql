alter table Customs.Declarations add DepartmentId varchar(15) null
go
alter table [Customs].[Declarations] add constraint [Declaration_Department] foreign key ([DepartmentId]) references [dbo].[Departments]([Id]);
