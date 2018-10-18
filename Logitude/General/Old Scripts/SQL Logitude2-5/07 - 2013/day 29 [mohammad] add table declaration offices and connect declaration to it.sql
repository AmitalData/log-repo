create table [Customs].[DeclarationOffices] (
    [Code] [varchar](17) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](100) null,
    primary key ([Code])
);
go

alter table customs.declarations drop Declaration_DeclarationOffice
go
insert into customs.DeclarationOffices (Code,EnglishName,LocalName,SearchFields)values('ACD12354','Dummy Office','Dummy Office','ACD12354,Dummy Office')
go
update customs.Declarations set DeclarationOfficeCode='ACD12354'
go
alter table [Customs].[Declarations] add constraint [Declaration_DeclarationOffice] foreign key ([DeclarationOfficeCode]) references [Customs].[DeclarationOffices]([Code]);
go