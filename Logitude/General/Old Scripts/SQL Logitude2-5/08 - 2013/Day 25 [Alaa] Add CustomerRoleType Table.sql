create table [Customs].[CustomerRoleTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go
update  [Customs].[DeclarationPaymentMethods] set PayerActivityTypeCode = null
go
alter table [Customs].[DeclarationPaymentMethods] add constraint [DeclarationPaymentMethod_CustomerRoleType] foreign key ([PayerActivityTypeCode]) references [Customs].[CustomerRoleTypes]([Code]);
go

