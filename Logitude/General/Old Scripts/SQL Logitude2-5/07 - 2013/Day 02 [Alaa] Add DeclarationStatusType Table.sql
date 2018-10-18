SP_RENAME '[customs].[Declarations].[DeclarationStatusCode]' , 'DeclarationStatusTypeCode' , 'COLUMN'
go

Update [Customs].[Declarations] set DeclarationStatusTypeCode = null;
go

alter table [Customs].[Declarations] add constraint [Declaration_DeclarationStatusType] foreign key ([DeclarationStatusTypeCode]) references [Customs].[DeclarationStatusTypes]([Code]);
go



create table [Customs].[DeclarationStatusTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

