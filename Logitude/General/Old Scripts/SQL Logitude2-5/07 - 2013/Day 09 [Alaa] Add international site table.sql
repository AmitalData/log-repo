create table [Customs].[InternationalSites] (
    [Code] [varchar](17) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[Consignments] add constraint [Consignment_InternationalSite] foreign key ([LoadingPortCode]) references [Customs].[InternationalSites]([Code]);
